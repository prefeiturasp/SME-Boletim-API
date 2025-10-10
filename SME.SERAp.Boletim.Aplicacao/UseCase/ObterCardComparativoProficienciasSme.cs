using Amazon.Runtime.Internal;
using Elastic.Apm.Api;
using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterCardComparativoProficienciasSme : IObterCardComparativoProficienciasSme
    {
        private readonly IMediator mediator;

        public ObterCardComparativoProficienciasSme(IMediator mediator)
        {
            this.mediator = mediator;
        }


        public async Task<CardsProficienciaComparativoSmeDto> Executar(int anoLetivo, int disciplinaId, int anoEscolar, int? dreId = null, int? pagina = null, int? itensPorPagina = null)
        {


            //var dresAbrangenciaUsuarioLogado = await mediator
            //    .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            //var tipoPerfilUsuarioLogado = await mediator
            //    .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            //if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
            //    throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");


            var filtroDres = await retornaTodasAsDresSeDreIdIsNull(dreId, anoLetivo, disciplinaId, anoEscolar);

            var cardsProficienciaComparativoSmeDto = new CardsProficienciaComparativoSmeDto();
            var listaCardsDres = new List<CardComparativoProficienciaDre>();
            var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar));

            foreach (var dre in filtroDres.OrderBy(x => x).ToList())
                listaCardsDres.Add(await CriaCardComparativoDre(dre, anoLetivo, disciplinaId, anoEscolar, niveisProficiencia));

            List<CardComparativoProficienciaDre> itensOrdenados, itensPaginados;
            Paginacao(pagina, itensPorPagina, listaCardsDres, out itensOrdenados, out itensPaginados);

            cardsProficienciaComparativoSmeDto.Total = itensOrdenados.Count;
            cardsProficienciaComparativoSmeDto.Pagina = pagina.GetValueOrDefault(1);
            cardsProficienciaComparativoSmeDto.ItensPorPagina = itensPorPagina.GetValueOrDefault(itensOrdenados.Count);
            cardsProficienciaComparativoSmeDto.Dres = itensPaginados;
            return cardsProficienciaComparativoSmeDto;
        }

        private static void Paginacao(int? pagina, int? itensPorPagina, List<CardComparativoProficienciaDre> listaCardsDres, out List<CardComparativoProficienciaDre> itensOrdenados, out List<CardComparativoProficienciaDre> itensPaginados)
        {
            itensOrdenados = (listaCardsDres ?? new List<CardComparativoProficienciaDre>())
                .OrderBy(x => x.DreNome)
                .ToList();
            if (pagina.HasValue && itensPorPagina.HasValue && pagina > 0 && itensPorPagina > 0)
            {
                var skip = (pagina.Value - 1) * itensPorPagina.Value;
                itensPaginados = itensOrdenados
                    .Skip(skip)
                    .Take(itensPorPagina.Value)
                    .ToList();
            }
            else
            {
                itensPaginados = itensOrdenados;
            }
        }

        private async Task<CardComparativoProficienciaDre> CriaCardComparativoDre(int? dreId, int anoLetivo, int disciplinaId, int anoEscolar, IEnumerable<ObterNivelProficienciaDto> niveisProficiencia)
        {
            var proficienciasPsa = await mediator.Send(new ObterProficienciaProvaSaberesPorDreQuery(dreId, anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciaProvaSPAPorDreQuery(dreId, anoLetivo - 1, disciplinaId, anoEscolar - 1));

            var listaProdificiencasComparativaPorDre = new List<ProficienciaTabelaComparativaDre>();

            var proficienciaComparativaPspDreDto = new ProficienciaTabelaComparativaDre();
            var proficienciaPsp = listaProficienciasPsp?.FirstOrDefault();

            var cardComparativoDreDto = new CardComparativoProficienciaDre()
            {
                DreAbreviacao = proficienciaPsp?.DreAbreviacao,
                DreNome = proficienciaPsp?.DreNome,
                AplicacaoPsp = await MapeiaProficienciaDetalheDreDto(disciplinaId, niveisProficiencia, proficienciaPsp)
            };

            var aplicacoesPsa = new List<ProficienciaDetalheDreDto>();
            if (proficienciasPsa.Any())
            {
                foreach (var proficiencia in proficienciasPsa)
                    aplicacoesPsa.Add(await MapeiaProficienciaDetalheDreDto(disciplinaId, niveisProficiencia, proficiencia));
            }
            cardComparativoDreDto.AplicacoesPsa = aplicacoesPsa;
            cardComparativoDreDto.Variacao = calculaVariacao(proficienciasPsa, proficienciaPsp);
            return cardComparativoDreDto;


        }

        private async Task<ProficienciaDetalheDreDto> MapeiaProficienciaDetalheDreDto(int disciplinaId, IEnumerable<ObterNivelProficienciaDto> niveisProficiencia, ResultadoProeficienciaPorDre proficienciaPsp)
        {
            return new ProficienciaDetalheDreDto()
            {
                MediaProficiencia = proficienciaPsp != null ? Math.Round((decimal)proficienciaPsp?.MediaProficiencia, 2) : 0,
                NivelProficiencia = proficienciaPsp != null ? await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficienciaPsp?.MediaProficiencia, disciplinaId, niveisProficiencia)) : string.Empty,
                NomeAplicacao = proficienciaPsp?.NomeAplicacao,
                Periodo = proficienciaPsp?.Periodo,
                RealizaramProva = proficienciaPsp != null ? proficienciaPsp.RealizaramProva : 0,
                QuantidadeUes = proficienciaPsp != null ? proficienciaPsp.QuantidadeUes : 0
            };
        }

    

        private async Task<List<int?>> retornaTodasAsDresSeDreIdIsNull(int? dreId, int anoLetivo, int disciplinaId, int anoEscolar)
        {
            var dresId = new List<int?>();

            if (dreId == null || dreId <= 0)
            {
                var dres = await mediator.Send(new ObterDresComparativoSmeQuery(anoLetivo, disciplinaId, anoEscolar));
                dres.ToList().ForEach(dre => dresId.Add(Convert.ToInt32(dre.DreId)));
                return dresId;
            }

            dresId.Add(dreId);
            return dresId;


        }

        private static decimal calculaVariacao(IEnumerable<ResultadoProeficienciaPorDre> proficienciasPsa, ResultadoProeficienciaPorDre proficienciasPsp)
        {
            var proficienciaFinal = proficienciasPsa.LastOrDefault() != null ? proficienciasPsa.LastOrDefault().MediaProficiencia : 0;
            var mediaProficiencia = proficienciasPsp != null ? proficienciasPsp.MediaProficiencia : 0;
            var variacao = (decimal)BoletimExtensions.CalcularPercentual((decimal)proficienciaFinal, (decimal)mediaProficiencia);
            return variacao;
        }
    }
}
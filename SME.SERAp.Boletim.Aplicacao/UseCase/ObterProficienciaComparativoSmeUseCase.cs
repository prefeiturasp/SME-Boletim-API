using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoSmeUseCase : IObterProficienciaComparativoSmeUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;

        public ObterProficienciaComparativoSmeUseCase(IMediator mediator, IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.mediator = mediator;
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public async Task<GraficoComparativoSmeDto> Executar(int anoLetivo, int disciplinaId, int anoEscolar)
        {

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || TipoPerfil.Administrador != tipoPerfilUsuarioLogado.Value)
                throw new NaoAutorizadoException("Usuário não possui acesso.");
            var proficienciaDre = new ProficienciasGraficoComparativoDreDto();


            var proficienciasProvaSaberes = await mediator.Send(new ObterProficienciaPorSmeProvaSaberesQuery(anoLetivo, disciplinaId, anoEscolar));
            var proficienciasDresProvaSP = await mediator.Send(new ObterProficienciasPorSmeProvaSPQuery(anoLetivo - 1, disciplinaId, anoEscolar - 1));

            if (proficienciasProvaSaberes is null || !proficienciasProvaSaberes.Any())
                return null;


            var grafico = new GraficoComparativoSmeDto();
            grafico.TodasAplicacoesDisponiveis = MapeiaListaAplicacoes(proficienciasProvaSaberes, proficienciasDresProvaSP);
           
            var listaGraficoComparativoSmeDto = new List<ProficienciasGraficoComparativoSmeDto>();
            var aplicacoesProvasPsaSme = proficienciasProvaSaberes.GroupBy(x => x.DreAbreviacao).ToList();
            
            foreach (var aplicacoesProvaPsaDre in aplicacoesProvasPsaSme)
            {
                var proficienciaGraficoComparativoSme = new ProficienciasGraficoComparativoSmeDto();
                var listaProficienciaGraficoComparativoDto = new List<ProficienciasGraficoComparativoDreDto>();
                proficienciaGraficoComparativoSme.ListaProficienciaGraficoComparativoDto = listaProficienciaGraficoComparativoDto;
               
                var proficienciaPSPGraficoComparativoDre = MapeamentoProvaSP(proficienciasDresProvaSP, aplicacoesProvaPsaDre, proficienciaGraficoComparativoSme);
                listaProficienciaGraficoComparativoDto.Add(proficienciaPSPGraficoComparativoDre);

                foreach (var aplicacaoDre in aplicacoesProvaPsaDre)
                {
                    var proficienciaPSAGraficoComparativoDre = MapeamentoProvaPSA(proficienciaGraficoComparativoSme, aplicacaoDre);
                    listaProficienciaGraficoComparativoDto.Add(proficienciaPSAGraficoComparativoDre);
                }

                listaGraficoComparativoSmeDto.Add(proficienciaGraficoComparativoSme);
            }
            grafico.Dados = listaGraficoComparativoSmeDto;
            return grafico;
        }

        private static ProficienciasGraficoComparativoDreDto MapeamentoProvaPSA(ProficienciasGraficoComparativoSmeDto proficienciaGraficoComparativoSme,  ResultadoProeficienciaPorDre itemDre)
        {
            var proficienciaPSAGraficoComparativoDre = new ProficienciasGraficoComparativoDreDto();
            proficienciaPSAGraficoComparativoDre.Mes = $"{itemDre.Periodo} PSP";
            proficienciaPSAGraficoComparativoDre.Descricao = itemDre.NomeAplicacao;
            proficienciaPSAGraficoComparativoDre.ValorProficiencia = itemDre.MediaProficiencia;
           

            if (string.IsNullOrEmpty(proficienciaGraficoComparativoSme.DreNome))
                proficienciaGraficoComparativoSme.DreNome = itemDre.DreNome;

            return proficienciaPSAGraficoComparativoDre;
        }

        private static ProficienciasGraficoComparativoDreDto MapeamentoProvaSP(IEnumerable<ResultadoProeficienciaPorDre> proficienciasDresProvaSP, IGrouping<string, ResultadoProeficienciaPorDre> dre, ProficienciasGraficoComparativoSmeDto proficienciaGraficoComparativoSme)
        {
            var proficienciaPSPGraficoComparativoDre = new ProficienciasGraficoComparativoDreDto();
            var proficienciasDreProvaSP = proficienciasDresProvaSP.Where(x => x.DreAbreviacao == dre.Key).FirstOrDefault();
            proficienciaPSPGraficoComparativoDre.Mes = $"{proficienciasDreProvaSP.Periodo} PSA";
            proficienciaPSPGraficoComparativoDre.Descricao = proficienciasDreProvaSP.NomeAplicacao;
            proficienciaPSPGraficoComparativoDre.ValorProficiencia = proficienciasDreProvaSP.MediaProficiencia;
            proficienciaGraficoComparativoSme.DreAbreviacao = dre.Key;
            proficienciaGraficoComparativoSme.DreNome = proficienciasDreProvaSP.DreNome;
            return proficienciaPSPGraficoComparativoDre;
        }

        private static List<string> MapeiaListaAplicacoes(IEnumerable<ResultadoProeficienciaPorDre> proficienciasProvaSaberes, IEnumerable<ResultadoProeficienciaPorDre> proficienciasDresProvaSP)
        {
            var listaAplicacoesDisponives = new List<string>();
            if (proficienciasProvaSaberes.Any())
                listaAplicacoesDisponives = proficienciasProvaSaberes.OrderBy(y => y.LoteId).Select(x => $"{x.Periodo} PSA").Distinct().ToList();

            if (proficienciasDresProvaSP.Any())
                listaAplicacoesDisponives.Add($"{proficienciasDresProvaSP?.FirstOrDefault().Periodo} PSP");

            return listaAplicacoesDisponives;
        }

    }
}
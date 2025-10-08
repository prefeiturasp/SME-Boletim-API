using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoDreUseCase : IObterProficienciaComparativoDreUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaComparativoDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TabelaComparativaDrePspPsaDto> Executar(int dreId, int anoLetivo, int disciplinaId, int anoEscolar)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            var proficienciasPsa = await mediator.Send(new ObterProficienciaProvaSaberesPorDreQuery(dreId, anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciaProvaSPAPorDreQuery(dreId, anoLetivo - 1, disciplinaId, anoEscolar - 1));
            var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar));

            var listaProdificiencasComparativaPorDre = new List<ProficienciaTabelaComparativaDre>();

            var proficienciaComparativaPspDreDto = new ProficienciaTabelaComparativaDre();
            var proficienciasPsp = listaProficienciasPsp.FirstOrDefault();

            proficienciaComparativaPspDreDto.Descricao = proficienciasPsp?.NomeAplicacao;
            proficienciaComparativaPspDreDto.Mes = proficienciasPsp?.Periodo;
            proficienciaComparativaPspDreDto.ValorProficiencia = proficienciasPsp != null ? Math.Round((decimal)proficienciasPsp?.MediaProficiencia, 2) : 0;
            proficienciaComparativaPspDreDto.NivelProficiencia = proficienciasPsp != null ? await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficienciasPsp?.MediaProficiencia, disciplinaId, niveisProficiencia)) : string.Empty;
            proficienciaComparativaPspDreDto.QtdeEstudante = proficienciasPsp != null ? proficienciasPsp.RealizaramProva : 0;
            proficienciaComparativaPspDreDto.QtdeUe = proficienciasPsp != null ? proficienciasPsp.QuantidadeUes : 0;
            listaProdificiencasComparativaPorDre.Add(proficienciaComparativaPspDreDto);

            if (proficienciasPsa.Any())
            {
                foreach (var proficiencia in proficienciasPsa)
                {
                    var proficienciaComparativaPsaDreDto = new ProficienciaTabelaComparativaDre();

                    proficienciaComparativaPsaDreDto.Descricao = proficiencia.NomeAplicacao;
                    proficienciaComparativaPsaDreDto.Mes = proficiencia.Periodo;
                    proficienciaComparativaPsaDreDto.ValorProficiencia = Math.Round((decimal)proficiencia.MediaProficiencia, 2);
                    proficienciaComparativaPsaDreDto.NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficiencia.MediaProficiencia, disciplinaId, niveisProficiencia));
                    proficienciaComparativaPsaDreDto.QtdeEstudante = proficiencia.RealizaramProva;
                    proficienciaComparativaPsaDreDto.QtdeUe = proficiencia.QuantidadeUes;
                    listaProdificiencasComparativaPorDre.Add(proficienciaComparativaPsaDreDto);
                }

            }

            decimal variacao = calculaVariacao(proficienciasPsa, proficienciasPsp);

            var tabelaRetorno = new TabelaComparativaDrePspPsaDto();
            tabelaRetorno.Variacao = variacao;
            tabelaRetorno.Aplicacao = listaProdificiencasComparativaPorDre;
            return tabelaRetorno;
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
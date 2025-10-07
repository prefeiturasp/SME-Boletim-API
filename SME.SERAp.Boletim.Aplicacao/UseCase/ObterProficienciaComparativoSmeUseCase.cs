using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSP;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoSmeUseCase : IObterProficienciaComparativoSmeUseCase
    {
        private readonly IMediator mediator;
        public ObterProficienciaComparativoSmeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TabelaComparativaSmePspPsaDto> Executar(int anoLetivo, int disciplinaId, int anoEscolar)
        {
            var proficienciasPsa = await mediator.Send(new ObterProficienciaSmeProvaSaberesQuery(anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciaSmeProvaSPQuery(anoLetivo - 1, disciplinaId, anoEscolar - 1));
            var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar));

            var listaProdificiencasComparativaSme = new List<ProficienciaTabelaComparativaSmeDto>();

            var proficienciaComparativaPspSmeDto = new ProficienciaTabelaComparativaSmeDto();
            var proficienciasPsp = listaProficienciasPsp.FirstOrDefault();

            proficienciaComparativaPspSmeDto.Descricao = proficienciasPsp?.NomeAplicacao;
            proficienciaComparativaPspSmeDto.Mes = proficienciasPsp?.Periodo;
            proficienciaComparativaPspSmeDto.ValorProficiencia = proficienciasPsp != null ? Math.Round((decimal)proficienciasPsp?.MediaProficiencia, 2) : 0;
            proficienciaComparativaPspSmeDto.NivelProficiencia = proficienciasPsp != null ? await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficienciasPsp?.MediaProficiencia, disciplinaId, niveisProficiencia)) : string.Empty;
            proficienciaComparativaPspSmeDto.QtdeEstudante = proficienciasPsp != null ? proficienciasPsp.RealizaramProva : 0;
            proficienciaComparativaPspSmeDto.QtdeUe = proficienciasPsp != null ? proficienciasPsp.QuantidadeUes : 0;
            proficienciaComparativaPspSmeDto.QtdeDre = proficienciasPsp != null ? proficienciasPsp.QuantidadeDres : 0;
            listaProdificiencasComparativaSme.Add(proficienciaComparativaPspSmeDto);

            if (proficienciasPsa.Any())
            {
                foreach (var proficiencia in proficienciasPsa)
                {
                    var proficienciaComparativaPsaDreDto = new ProficienciaTabelaComparativaSmeDto();

                    proficienciaComparativaPsaDreDto.Descricao = proficiencia.NomeAplicacao;
                    proficienciaComparativaPsaDreDto.Mes = proficiencia.Periodo;
                    proficienciaComparativaPsaDreDto.ValorProficiencia = Math.Round((decimal)proficiencia.MediaProficiencia, 2);
                    proficienciaComparativaPsaDreDto.NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficiencia.MediaProficiencia, disciplinaId, niveisProficiencia));
                    proficienciaComparativaPsaDreDto.QtdeEstudante = proficiencia.RealizaramProva;
                    proficienciaComparativaPsaDreDto.QtdeUe = proficiencia.QuantidadeUes;
                    proficienciaComparativaPsaDreDto.QtdeDre = proficiencia.QuantidadeDres;
                    listaProdificiencasComparativaSme.Add(proficienciaComparativaPsaDreDto);
                }
            }

            decimal variacao = calculaVariacao(proficienciasPsa, proficienciasPsp);

            var tabelaRetorno = new TabelaComparativaSmePspPsaDto();
            tabelaRetorno.Variacao = variacao;
            tabelaRetorno.Aplicacao = listaProdificiencasComparativaSme;
            return tabelaRetorno;
        }

        private static decimal calculaVariacao(IEnumerable<ResultadoProeficienciaSme> proficienciasPsa, ResultadoProeficienciaSme proficienciasPsp)
        {
            var proficienciaFinal = proficienciasPsa.LastOrDefault() != null ? proficienciasPsa.LastOrDefault().MediaProficiencia : 0;
            var mediaProficiencia = proficienciasPsp != null ? proficienciasPsp.MediaProficiencia : 0;
            var variacao = (decimal)BoletimExtensions.CalcularPercentual((decimal)proficienciaFinal, (decimal)mediaProficiencia);
            return variacao;
        }
    }
}

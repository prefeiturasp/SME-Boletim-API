using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoDreUseCase : IObterProficienciaComparativoDreUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;

        public ObterProficienciaComparativoDreUseCase(IMediator mediator, IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.mediator = mediator;
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public async Task<TabelaComparativaDrePspPsaDto> Executar(int dreId, int anoLetivo  , int disciplinaId, int anoEscolar)
        {


            var proficienciasPsa = await mediator.Send(new ObterProficienciaProvaSaberesPorDreQuery(dreId, anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciaProvaSPAPorDreQuery(dreId, anoLetivo - 1, disciplinaId, anoEscolar - 1));
            var niveisProficiencia = await repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar);

            var listaProdificiencasComparativaPorDre = new List<ProficienciaTabelaComparativaDre>();

            var proficienciaComparativaPspDreDto = new ProficienciaTabelaComparativaDre();
            var proficienciasPsp = listaProficienciasPsp.FirstOrDefault();

            proficienciaComparativaPspDreDto.Descricao = proficienciasPsp?.NomeAplicacao;
            proficienciaComparativaPspDreDto.Mes = proficienciasPsp?.Periodo;
            proficienciaComparativaPspDreDto.ValorProficiencia = Math.Round((decimal)proficienciasPsp.MediaProficiencia, 2);
            proficienciaComparativaPspDreDto.NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficienciasPsp?.MediaProficiencia, disciplinaId, niveisProficiencia));
            proficienciaComparativaPspDreDto.QtdeEstudante = proficienciasPsp.RealizaramProva;
            proficienciaComparativaPspDreDto.QtdeUe = proficienciasPsp.QuantidadeUes;



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
            var variacao = (decimal)BoletimExtensions.CalcularPercentual((decimal)proficienciaFinal, (decimal)proficienciasPsp.MediaProficiencia);
            return variacao;
        }
    }
}
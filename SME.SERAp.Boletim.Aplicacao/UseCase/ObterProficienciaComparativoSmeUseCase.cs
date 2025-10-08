using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    internal class ObterProficienciaComparativoSmeUseCase : IObterProficienciaComparativoDreUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;

        public ObterProficienciaComparativoSmeUseCase(IMediator mediator, IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.mediator = mediator;
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public async Task<IEnumerable<ProficienciaTabelaComparativaDre>> Executar(int anoLetivo, int disciplinaId, int anoEscolar)
        {


            var proficienciasPsa = await mediator.Send(new ObterProficienciaPorSmeProvaSaberesQuery(anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciasPorSmeProvaSPQuery(anoLetivo - 1, disciplinaId, anoEscolar - 1));
            var niveisProficiencia = await repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar);

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
           tabelaRetorno;
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
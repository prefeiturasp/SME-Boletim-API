using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP
{
    public class ObterNiveisProficienciaComparativoProvaSPQueryHandler : IRequestHandler<ObterNiveisProficienciaComparativoProvaSPQuery, ProficienciaUeComparacaoProvaSPDto>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        private readonly IMediator mediator;

        public ObterNiveisProficienciaComparativoProvaSPQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar, IMediator mediator)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
            this.mediator = mediator;
        }

        public async Task<ProficienciaUeComparacaoProvaSPDto> Handle(ObterNiveisProficienciaComparativoProvaSPQuery request, CancellationToken cancellationToken)
        {
            var niveisProficiencia = await repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(request.DisciplinaId, request.AnoEscolar);

            var proficienciasAnoCorrente = await repositorioBoletimEscolar.ObterMediaProficienciaUeAsync(
                request.LoteId, request.UeId, request.DisciplinaId, request.AnoEscolar);

            var proficienciaAnoAnterior = (await repositorioBoletimEscolar.ObterMediaProficienciaUeAnoAnteriorAsync(
                request.LoteId, request.UeId, request.DisciplinaId, request.AnoEscolar)).FirstOrDefault();

            var proficienciaSp = new ProficienciaProvaSpDto();
            if (proficienciaAnoAnterior != null)
            {
                var totalAlunosRealizaramProvaAnoAnterior = await repositorioBoletimEscolar.ObterTotalAlunosUeRealizaramProvasSPAnterior(request.LoteId, request.UeId, request.DisciplinaId, request.AnoEscolar);
                var nivelProficienciaAnterior = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery(proficienciaAnoAnterior.MediaProficiencia, request.DisciplinaId, niveisProficiencia));

                proficienciaSp = new ProficienciaProvaSpDto
                {
                    LoteId = proficienciaAnoAnterior.LoteId,
                    NomeAplicacao = proficienciaAnoAnterior.NomeAplicacao,
                    Periodo = proficienciaAnoAnterior.Periodo,
                    MediaProficiencia = proficienciaAnoAnterior.MediaProficiencia,
                    NivelProficiencia = nivelProficienciaAnterior,
                    TotalRealizaramProva = totalAlunosRealizaramProvaAnoAnterior
                };
            }
            else
            {
                proficienciaSp = new ProficienciaProvaSpDto
                {
                    LoteId = 0,
                    NomeAplicacao = "Não Avaliado",
                    Periodo = "Não Avaliado",
                    MediaProficiencia = 0,
                    NivelProficiencia = "Não Avaliado",
                    TotalRealizaramProva = 0
                };
            }

            var itens = new List<ProficienciaUeDto>();
            foreach (var proficiencia in proficienciasAnoCorrente)
            {
                var totalAlunosRealizaramProvaAnoCorrente = await repositorioBoletimEscolar.ObterTotalAlunosRealizaramProvasUe(proficiencia.LoteId, request.AnoEscolar, request.UeId);
                var nivelProficienciaCorrente = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery(proficiencia.MediaProficiencia, request.DisciplinaId, niveisProficiencia));

                itens.Add(new ProficienciaUeDto
                {
                    LoteId = proficiencia.LoteId,
                    NomeAplicacao = proficiencia.NomeAplicacao,
                    Periodo = proficiencia.Periodo,
                    MediaProficiencia = proficiencia.MediaProficiencia,
                    NivelProficiencia = nivelProficienciaCorrente,
                    TotalRealizaramProva = totalAlunosRealizaramProvaAnoCorrente
                });
            }

            return new ProficienciaUeComparacaoProvaSPDto
            {
                TotalLotes = itens.Count(),
                ProvaSP = proficienciaSp,
                Lotes = itens.OrderByDescending(x => x.Periodo)
            };
        }
    }
}
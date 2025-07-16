using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarResumoDreUseCase : IObterBoletimEscolarResumoDreUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimEscolarResumoDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarResumoDreDto> Executar(long loteId, long dreId, int anoEscolar)
        {
            var totalUes = await mediator.Send(new ObterTotalUesPorDreQuery(loteId, dreId, anoEscolar));
            var totalAlunos = await mediator.Send(new ObterTotalAlunosPorDreQuery(loteId, dreId, anoEscolar));
            var proficiencias = await mediator.Send(new ObterMediaProficienciaPorDreQuery(loteId, dreId, anoEscolar));

            return new BoletimEscolarResumoDreDto
            {
                TotalUes = totalUes,
                TotalAlunos = totalAlunos,
                ProficienciaDisciplina = proficiencias
            };
        }
    }
}
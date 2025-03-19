using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAbaEstudanteGraficoPorUeIdUseCase : IObterAbaEstudanteGraficoPorUeIdUseCase
    {
        private readonly IMediator _mediator;

        public ObterAbaEstudanteGraficoPorUeIdUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> Executar(long ueId)
        {
            return await _mediator.Send(new ObterAbaEstudanteGraficoPorUeIdQuery(ueId));
        }
    }
}
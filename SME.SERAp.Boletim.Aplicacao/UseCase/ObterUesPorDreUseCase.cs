using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterUesPorDreUseCase : IObterUesPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterUesPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UePorDreDto>> Executar(long dreId, int anoEscolar, long loteId)
        {
            return await mediator.Send(new ObterUesPorDreQuery(dreId, anoEscolar, loteId));
        }
    }
}
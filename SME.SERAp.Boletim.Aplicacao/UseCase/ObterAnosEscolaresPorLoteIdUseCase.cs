using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAnosEscolaresPorLoteIdUseCase : IObterAnosEscolaresPorLoteIdUseCase
    {
        private readonly IMediator mediator;

        public ObterAnosEscolaresPorLoteIdUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<AnoEscolarDto>> Executar(long loteId)
        {
            return await mediator.Send(new ObterAnosEscolaresPorLoteIdQuery(loteId));
        }
    }
}

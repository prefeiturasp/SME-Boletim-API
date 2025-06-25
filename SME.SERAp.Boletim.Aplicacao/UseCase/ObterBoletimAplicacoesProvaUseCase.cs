using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProva;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimAplicacoesProvaUseCase : IObterBoletimAplicacoesProvaUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimAplicacoesProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<IEnumerable<LoteProvaDto>> Executar()
        {
            return mediator.Send(new ObterLotesProvaQuery());
        }
    }
}

using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public abstract class AbstractUseCase
    {
        protected readonly IMediator mediator;

        public AbstractUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}

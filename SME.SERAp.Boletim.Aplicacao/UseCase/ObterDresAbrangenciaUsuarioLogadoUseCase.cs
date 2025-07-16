using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDresAbrangenciaUsuarioLogadoUseCase : IObterDresAbrangenciaUsuarioLogadoUseCase
    {
        private readonly IMediator mediator;

        public ObterDresAbrangenciaUsuarioLogadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<IEnumerable<DreAbragenciaDetalheDto>> Executar()
        {
            return mediator.Send(new ObterDresAbrangenciaUsuarioLogadoQuery());
        }
    }
}

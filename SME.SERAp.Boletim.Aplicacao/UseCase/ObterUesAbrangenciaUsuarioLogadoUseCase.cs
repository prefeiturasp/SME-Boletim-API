using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterUesAbrangenciaUsuarioLogadoUseCase : IObterUesAbrangenciaUsuarioLogadoUseCase
    {
        private readonly IMediator _mediator;

        public ObterUesAbrangenciaUsuarioLogadoUseCase(IMediator meditor)
        {
            _mediator = meditor;
        }

        public Task<IEnumerable<AbrangenciaUeDto>> Executar()
        {
            return _mediator.Send(new ObterUesAbrangenciaUsuarioLogadoQuery());
        }
    }
}

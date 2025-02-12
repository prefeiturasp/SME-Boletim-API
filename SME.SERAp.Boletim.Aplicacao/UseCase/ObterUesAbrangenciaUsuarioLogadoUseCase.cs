using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterUesAbrangenciaUsuarioLogadoUseCase : IObterUesAbrangenciaUsuarioLogadoUseCase
    {
        private readonly IMediator _meditor;

        public ObterUesAbrangenciaUsuarioLogadoUseCase(IMediator meditor)
        {
            _meditor = meditor;
        }

        public Task<IEnumerable<AbrangenciaUeDto>> Executar()
        {
            return _meditor.Send(new ObterUesAbrangenciaUsuarioLogadoQuery());
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado
{
    public class ObterUesAbrangenciaUsuarioLogadoQuery : IRequest<IEnumerable<AbrangenciaUeDto>>
    {
    }
}

using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDresAbrangenciaUsuarioLogadoQuery : IRequest<IEnumerable<DreAbragenciaDetalheDto>>
    {
    }
}

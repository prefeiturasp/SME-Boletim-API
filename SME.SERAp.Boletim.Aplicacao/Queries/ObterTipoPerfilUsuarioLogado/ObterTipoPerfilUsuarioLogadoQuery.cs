using MediatR;
using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTipoPerfilUsuarioLogadoQuery : IRequest<TipoPerfil?>
    {
    }
}

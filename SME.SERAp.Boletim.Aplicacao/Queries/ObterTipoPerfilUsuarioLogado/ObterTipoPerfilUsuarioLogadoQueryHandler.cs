using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTipoPerfilUsuarioLogadoQueryHandler : IRequestHandler<ObterTipoPerfilUsuarioLogadoQuery, TipoPerfil?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ObterTipoPerfilUsuarioLogadoQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TipoPerfil?> Handle(ObterTipoPerfilUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var perfilUsuarioLogadoString = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "PERFIL")?.Value;
            if (perfilUsuarioLogadoString is null || !Guid.TryParse(perfilUsuarioLogadoString, out var perfilUsuarioLogado))
                return null;

            return Perfis.ObterTipoPerfil(perfilUsuarioLogado);
        }
    }
}

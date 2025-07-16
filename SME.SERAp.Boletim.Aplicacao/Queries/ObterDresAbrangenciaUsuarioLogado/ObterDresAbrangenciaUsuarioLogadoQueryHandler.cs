using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDresAbrangenciaUsuarioLogadoQueryHandler : IRequestHandler<ObterDresAbrangenciaUsuarioLogadoQuery, IEnumerable<DreAbragenciaDetalheDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositorioAbrangencia _repositorioAbrangencia;
        private readonly IRepositorioCache _repositorioCache;
        public ObterDresAbrangenciaUsuarioLogadoQueryHandler(IHttpContextAccessor httpContextAccessor, IRepositorioAbrangencia repositorioAbrangencia, IRepositorioCache repositorioCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositorioAbrangencia = repositorioAbrangencia;
            _repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<DreAbragenciaDetalheDto>> Handle(ObterDresAbrangenciaUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var loginUsuarioLogado = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "LOGIN")?.Value;
            if (loginUsuarioLogado is null)
                return default;

            var perfilUsuarioLogadoString = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "PERFIL")?.Value;
            if (perfilUsuarioLogadoString is null || !Guid.TryParse(perfilUsuarioLogadoString, out var perfilUsuarioLogado))
                return default;

            var chaveCache = string.Format(CacheChave.UsuarioLoginPerfilDresAbrangencia, loginUsuarioLogado, perfilUsuarioLogado);
            var dresAbrangenciaCache = await ObterDresAbragenciaUsuarioCache(chaveCache);
            if (dresAbrangenciaCache?.Any() ?? false)
                return dresAbrangenciaCache;

            var dresAbrangencia = Perfis.PerfilEhAdministrador(perfilUsuarioLogado)
                    ? await _repositorioAbrangencia.ObterDresAdministrador()
                    : await _repositorioAbrangencia.ObterDresAbrangenciaPorLoginPerfil(loginUsuarioLogado, perfilUsuarioLogado);

            await SalvarDresAbrangenciaUsuarioCache(chaveCache, dresAbrangencia);
            return dresAbrangencia;
        }

        private async Task SalvarDresAbrangenciaUsuarioCache(string chaveCache, IEnumerable<DreAbragenciaDetalheDto> dresAbrangencia)
        {
            await _repositorioCache.SalvarRedisToJsonAsync(chaveCache, JsonSerializer.Serialize(dresAbrangencia));
        }

        private async Task<IEnumerable<DreAbragenciaDetalheDto>> ObterDresAbragenciaUsuarioCache(string chaveCache)
        {
            var dresAbrangenciaCacheJson = await _repositorioCache.ObterRedisToJsonAsync(chaveCache);
            if (!string.IsNullOrEmpty(dresAbrangenciaCacheJson))
                return JsonSerializer.Deserialize<IEnumerable<DreAbragenciaDetalheDto>>(dresAbrangenciaCacheJson);

            return default;
        }
    }
}

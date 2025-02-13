using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado
{
    public class ObterUesAbrangenciaUsuarioLogadoQueryHandler :
        IRequestHandler<ObterUesAbrangenciaUsuarioLogadoQuery, IEnumerable<AbrangenciaUeDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositorioAbrangencia _repositorioAbrangencia;
        private readonly IRepositorioCache _repositorioCache;
        public ObterUesAbrangenciaUsuarioLogadoQueryHandler(IHttpContextAccessor httpContextAccessor, IRepositorioAbrangencia repositorioAbrangencia, IRepositorioCache repositorioCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositorioAbrangencia = repositorioAbrangencia;
            _repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<AbrangenciaUeDto>> Handle(ObterUesAbrangenciaUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var loginUsuarioLogado = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "LOGIN")?.Value;
            if (loginUsuarioLogado is null)
                return default;

            var chaveUsuarioLoginUeAbrangencia = string.Format(CacheChave.UsuarioLoginUeAbrangencia, loginUsuarioLogado);
            return await _repositorioCache.ObterRedisAsync(chaveUsuarioLoginUeAbrangencia, async () => await ObterUesUsuarioLogado(loginUsuarioLogado));
        }

        private async Task<List<AbrangenciaUeDto>> ObterUesUsuarioLogado(string loginUsuarioLogado)
        {
            var abrangenciasUsuarioLogado = await _repositorioAbrangencia.ObterAbrangenciaPorLogin(loginUsuarioLogado);
            if (!abrangenciasUsuarioLogado?.Any() ?? true)
                return default;

            var ues = new List<AbrangenciaUeDto>();
            var queries = new List<Task>();
            foreach (var abrangencia in abrangenciasUsuarioLogado.GroupBy(a => new { a.DreId, a.UeId })
                    .Select(a => new { a.Key.DreId, a.Key.UeId }))
            {
                queries.Add(Task.Run(async () =>
                {
                    var uesAbrangencia = await _repositorioAbrangencia.ObterUesPorAbrangenciaDre(abrangencia.DreId, abrangencia.UeId);
                    if (uesAbrangencia?.Any() ?? false)
                        ues.AddRange(uesAbrangencia);
                }));
            }

            await Task.WhenAll(queries);
            return ues;
        }
    }
}

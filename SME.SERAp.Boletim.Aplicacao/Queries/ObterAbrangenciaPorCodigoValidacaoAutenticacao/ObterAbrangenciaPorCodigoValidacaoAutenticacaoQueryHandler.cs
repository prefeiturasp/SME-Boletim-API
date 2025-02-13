using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao
{
    public class ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandler : IRequestHandler<ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery, IEnumerable<AbrangenciaDetalheDto>>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<AbrangenciaDetalheDto>> Handle(ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery request, CancellationToken cancellationToken)
        {
            var chave = string.Format(CacheChave.Autenticacao, request.Codigo);

            var abrangencias = await repositorioCache.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave);

            if (abrangencias == null || !abrangencias.Any()) return default;

            return abrangencias;
        }
    }
}

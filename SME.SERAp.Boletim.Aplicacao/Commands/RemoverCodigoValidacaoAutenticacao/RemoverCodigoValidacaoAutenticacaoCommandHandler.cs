using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;

namespace SME.SERAp.Boletim.Aplicacao.Commands.RemoverCodigoValidacaoAutenticacao
{
    public class RemoverCodigoValidacaoAutenticacaoCommandHandler : IRequestHandler<RemoverCodigoValidacaoAutenticacaoCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverCodigoValidacaoAutenticacaoCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(RemoverCodigoValidacaoAutenticacaoCommand request, CancellationToken cancellationToken)
        {
            var chave = string.Format(CacheChave.Autenticacao, request.Codigo);
            await repositorioCache.RemoverRedisAsync(chave);
            return true;
        }
    }
}

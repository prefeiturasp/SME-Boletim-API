using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao
{
    public class GerarCodigoValidacaoAutenticacaoCommandHandler : IRequestHandler<GerarCodigoValidacaoAutenticacaoCommand, AutenticacaoValidarDto>
    {
        public readonly IRepositorioCache repositorioCache;

        public GerarCodigoValidacaoAutenticacaoCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<AutenticacaoValidarDto> Handle(GerarCodigoValidacaoAutenticacaoCommand request, CancellationToken cancellationToken)
        {
            var codigo = Guid.NewGuid();
            var chave = string.Format(CacheChave.Autenticacao, codigo);

            await repositorioCache.SalvarRedisAsync(chave, request.Abrangencias);

            return new AutenticacaoValidarDto(codigo.ToString());
        }
    }
}

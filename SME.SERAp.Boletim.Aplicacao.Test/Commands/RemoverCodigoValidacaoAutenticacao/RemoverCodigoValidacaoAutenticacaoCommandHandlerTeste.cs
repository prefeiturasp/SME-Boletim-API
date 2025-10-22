using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.RemoverCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;

namespace SME.SERAp.Boletim.Aplicacao.Test.Commands
{
    public class RemoverCodigoValidacaoAutenticacaoCommandHandlerTeste
    {
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly RemoverCodigoValidacaoAutenticacaoCommandHandler handler;

        public RemoverCodigoValidacaoAutenticacaoCommandHandlerTeste()
        {
            this.repositorioCache = new Mock<IRepositorioCache>();
            this.handler = new RemoverCodigoValidacaoAutenticacaoCommandHandler(repositorioCache.Object);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_RepositorioCache_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RemoverCodigoValidacaoAutenticacaoCommandHandler(null));
        }

        [Fact]
        public async Task Deve_Chamar_RemoverRedisAsync_Com_Chave_Correta()
        {
            var codigo = Guid.NewGuid().ToString();
            var comando = new RemoverCodigoValidacaoAutenticacaoCommand(codigo);

            var resultado = await handler.Handle(comando, CancellationToken.None);

            var chaveEsperada = string.Format(CacheChave.Autenticacao, codigo);

            repositorioCache
                .Verify(x => x.RemoverRedisAsync(chaveEsperada), Times.Once);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Verdadeiro_Ao_Remover_Codigo()
        {
            var codigo = Guid.NewGuid().ToString();
            var comando = new RemoverCodigoValidacaoAutenticacaoCommand(codigo);

            var resultado = await handler.Handle(comando, CancellationToken.None);

            Assert.True(resultado);
        }
    }
}

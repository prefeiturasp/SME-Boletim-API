using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Test.Commands
{
    public class GerarCodigoValidacaoAutenticacaoCommandHandlerTeste
    {
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly GerarCodigoValidacaoAutenticacaoCommandHandler handler;
        public GerarCodigoValidacaoAutenticacaoCommandHandlerTeste()
        {
            this.repositorioCache = new Mock<IRepositorioCache>();
            this.handler = new GerarCodigoValidacaoAutenticacaoCommandHandler(repositorioCache.Object);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_RepositorioCache_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new GerarCodigoValidacaoAutenticacaoCommandHandler(null));
        }

        [Fact]
        public async Task Deve_Chamar_SalvarRedisAsync_Com_Chave_E_Abrangencias()
        {
            var abrangencias = new List<AbrangenciaDetalheDto>();
            var comando = new GerarCodigoValidacaoAutenticacaoCommand(abrangencias);

            var resultado = await handler.Handle(comando, CancellationToken.None);

            repositorioCache
                .Verify(x => x.SalvarRedisAsync(It.Is<string>(chave => chave.Contains(resultado.Codigo)), comando.Abrangencias, 720), Times.Once);

            Assert.False(string.IsNullOrWhiteSpace(resultado.Codigo));
            Assert.True(Guid.TryParse(resultado.Codigo, out _));
        }

        [Fact]
        public async Task Deve_Retornar_AutenticacaoValidarDto_Com_Codigo()
        {
            var abrangencias = new List<AbrangenciaDetalheDto>();
            var comando = new GerarCodigoValidacaoAutenticacaoCommand(abrangencias);

            var resultado = await handler.Handle(comando, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.False(string.IsNullOrEmpty(resultado.Codigo));
            Assert.True(Guid.TryParse(resultado.Codigo, out _));
        }
    }
}

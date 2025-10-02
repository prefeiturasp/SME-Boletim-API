using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAnosAplicacaoPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterAnosAplicacaoPorDreQueryHandler handler;

        public ObterAnosAplicacaoPorDreQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterAnosAplicacaoPorDreQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_Anos_Correta()
        {
            var dreId = 123L;
            var anosEsperados = new List<int> { 2021, 2022, 2023 };

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorDre(dreId))
                .ReturnsAsync(anosEsperados);

            var query = new ObterAnosAplicacaoPorDreQuery(dreId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anosEsperados.Count, resultado.Count());
            Assert.Equal(anosEsperados, resultado);

            repositorio.Verify(r => r.ObterAnosAplicacaoPorDre(dreId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Tiver_Dados()
        {
            var dreId = 456L;

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorDre(dreId))
                .ReturnsAsync(new List<int>());

            var query = new ObterAnosAplicacaoPorDreQuery(dreId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorio.Verify(r => r.ObterAnosAplicacaoPorDre(dreId), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Repositorio_Falhar()
        {
            var dreId = 999L;
            var query = new ObterAnosAplicacaoPorDreQuery(dreId);

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorDre(dreId))
                .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", exception.Message);

            repositorio.Verify(r => r.ObterAnosAplicacaoPorDre(dreId), Times.Once);
        }
    }
}

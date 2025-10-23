using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalUesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTotalUesPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterTotalUesPorDreQueryHandler handler;

        public ObterTotalUesPorDreQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterTotalUesPorDreQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Valor_Correto_Quando_Existir()
        {
            repositorioMock
                .Setup(r => r.ObterTotalUesPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(25);

            var requisicao = new ObterTotalUesPorDreQuery(10, 2, 5);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.Equal(25, resultado);
            repositorioMock.Verify(r =>
                r.ObterTotalUesPorDreAsync(10, 2, 5), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Zero_Quando_Nao_Houver_Ues()
        {
            repositorioMock
                .Setup(r => r.ObterTotalUesPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(0);

            var requisicao = new ObterTotalUesPorDreQuery(11, 3, 9);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.Equal(0, resultado);
            repositorioMock.Verify(r =>
                r.ObterTotalUesPorDreAsync(11, 3, 9), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorioMock
                .Setup(r => r.ObterTotalUesPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterTotalUesPorDreQuery(12, 4, 8);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorioMock.Verify(r =>
                r.ObterTotalUesPorDreAsync(12, 4, 8), Times.Once);
        }
    }
}

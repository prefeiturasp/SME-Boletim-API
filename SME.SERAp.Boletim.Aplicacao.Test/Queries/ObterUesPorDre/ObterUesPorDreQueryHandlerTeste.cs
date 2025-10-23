using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterUesPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterUesPorDreQueryHandler handler;

        public ObterUesPorDreQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterUesPorDreQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Quando_Existirem()
        {
            var uesEsperadas = new List<UePorDreDto>
            {
                new UePorDreDto { UeId = 1, UeNome = "Escola A" },
                new UePorDreDto { UeId = 2, UeNome = "Escola B" }
            };

            repositorioMock
                .Setup(r => r.ObterUesPorDreAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(uesEsperadas);

            var requisicao = new ObterUesPorDreQuery(3, 5, 10);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, r => r.UeNome == "Escola A");
            Assert.Contains(resultado, r => r.UeNome == "Escola B");

            repositorioMock.Verify(r =>
                r.ObterUesPorDreAsync(3, 5, 10), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Ues()
        {
            repositorioMock
                .Setup(r => r.ObterUesPorDreAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new List<UePorDreDto>());

            var requisicao = new ObterUesPorDreQuery(4, 9, 11);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioMock.Verify(r =>
                r.ObterUesPorDreAsync(4, 9, 11), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorioMock
                .Setup(r => r.ObterUesPorDreAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterUesPorDreQuery(5, 9, 12);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorioMock.Verify(r =>
                r.ObterUesPorDreAsync(5, 9, 12), Times.Once);
        }
    }
}

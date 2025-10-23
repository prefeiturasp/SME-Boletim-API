using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterProficienciasPorSmeProvaSPQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterProficienciasPorSmeProvaSPQueryHandler handler;

        public ObterProficienciasPorSmeProvaSPQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterProficienciasPorSmeProvaSPQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Quando_Existirem()
        {
            var resultadosEsperados = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = 10, DreNome = "DRE Norte 1", MediaProficiencia = 245.6m, LoteId = 1 },
                new ResultadoProeficienciaPorDre { DreId = 11, DreNome = "DRE Sul 2", MediaProficiencia = 312.8m, LoteId = 2 }
            };

            repositorioMock
                .Setup(r => r.ObterProficienciasPorSmeProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(resultadosEsperados);

            var requisicao = new ObterProficienciasPorSmeProvaSPQuery(2025, 1, 5);

            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, r => r.MediaProficiencia == 245.6m);
            Assert.Contains(resultado, r => r.MediaProficiencia == 312.8m);

            repositorioMock.Verify(r =>
                r.ObterProficienciasPorSmeProvaSPAsync(2025, 1, 5), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Resultados()
        {
            repositorioMock
                .Setup(r => r.ObterProficienciasPorSmeProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var requisicao = new ObterProficienciasPorSmeProvaSPQuery(2025, 2, 6);

            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioMock.Verify(r =>
                r.ObterProficienciasPorSmeProvaSPAsync(2025, 2, 6), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorioMock
                .Setup(r => r.ObterProficienciasPorSmeProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterProficienciasPorSmeProvaSPQuery(2025, 3, 7);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorioMock.Verify(r =>
                r.ObterProficienciasPorSmeProvaSPAsync(2025, 3, 7), Times.Once);
        }
    }
}

using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterProficienciaPorSmeProvaSaberesQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterProficienciaPorSmeProvaSaberesQueryHandler handler;

        public ObterProficienciaPorSmeProvaSaberesQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterProficienciaPorSmeProvaSaberesQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Quando_Existirem()
        {
            var resultadosEsperados = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = 1, DreNome = "DRE Norte", MediaProficiencia = 250.75m, LoteId = 10 },
                new ResultadoProeficienciaPorDre { DreId = 2, DreNome = "DRE Sul", MediaProficiencia = 310.25m, LoteId = 11 }
            };

            repositorioMock
                .Setup(r => r.ObterProficienciaPorSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(resultadosEsperados);

            var requisicao = new ObterProficienciaPorSmeProvaSaberesQuery(2024, 1, 5);

            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, r => r.MediaProficiencia == 250.75m);
            Assert.Contains(resultado, r => r.MediaProficiencia == 310.25m);

            repositorioMock.Verify(r =>
                r.ObterProficienciaPorSmeProvaSaberesAsync(2024, 1, 5), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Resultados()
        {
            repositorioMock
                .Setup(r => r.ObterProficienciaPorSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var requisicao = new ObterProficienciaPorSmeProvaSaberesQuery(2024, 2, 6);

            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioMock.Verify(r =>
                r.ObterProficienciaPorSmeProvaSaberesAsync(2024, 2, 6), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorioMock
                .Setup(r => r.ObterProficienciaPorSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterProficienciaPorSmeProvaSaberesQuery(2024, 3, 7);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorioMock.Verify(r =>
                r.ObterProficienciaPorSmeProvaSaberesAsync(2024, 3, 7), Times.Once);
        }
    }
}

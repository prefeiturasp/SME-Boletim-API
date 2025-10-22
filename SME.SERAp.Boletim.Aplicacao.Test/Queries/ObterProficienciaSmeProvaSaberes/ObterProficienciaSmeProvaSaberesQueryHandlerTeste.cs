using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSaberes;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterProficienciaSmeProvaSaberesQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimProvaAluno;
        private readonly ObterProficienciaSmeProvaSaberesQueryHandler handler;

        public ObterProficienciaSmeProvaSaberesQueryHandlerTeste()
        {
            repositorioBoletimProvaAluno = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterProficienciaSmeProvaSaberesQueryHandler(repositorioBoletimProvaAluno.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Quando_Existirem()
        {
            var resultadosEsperados = new List<ResultadoProeficienciaSme>
            {
                new ResultadoProeficienciaSme { QuantidadeDres = 1, QuantidadeUes = 1, DisciplinaId = 1, AnoEscolar = "5", MediaProficiencia = 230.5m, LoteId = 4 },
                new ResultadoProeficienciaSme { QuantidadeDres = 1, QuantidadeUes = 1, DisciplinaId = 1, AnoEscolar = "5", MediaProficiencia = 330.35m, LoteId = 5 }
            };

            repositorioBoletimProvaAluno
                .Setup(r => r.ObterProficienciaSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(resultadosEsperados);

            var requisicao = new ObterProficienciaSmeProvaSaberesQuery(2024, 1, 5);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, r => r.MediaProficiencia == 230.5m);
            Assert.Contains(resultado, r => r.MediaProficiencia == 330.35m);

            repositorioBoletimProvaAluno.Verify(r =>
                r.ObterProficienciaSmeProvaSaberesAsync(2024, 1, 5), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Resultados()
        {
            repositorioBoletimProvaAluno
                .Setup(r => r.ObterProficienciaSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ResultadoProeficienciaSme>());

            var requisicao = new ObterProficienciaSmeProvaSaberesQuery(2024, 2, 6);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioBoletimProvaAluno.Verify(r =>
                r.ObterProficienciaSmeProvaSaberesAsync(2024, 2, 6), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorioBoletimProvaAluno
                .Setup(r => r.ObterProficienciaSmeProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterProficienciaSmeProvaSaberesQuery(2024, 3, 7);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorioBoletimProvaAluno.Verify(r =>
                r.ObterProficienciaSmeProvaSaberesAsync(2024, 3, 7), Times.Once);
        }
    }
}

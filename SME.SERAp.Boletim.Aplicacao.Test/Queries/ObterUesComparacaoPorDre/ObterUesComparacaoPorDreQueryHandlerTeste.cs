using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterUesComparacaoPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterUesComparacaoPorDreQueryHandler handler;

        public ObterUesComparacaoPorDreQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterUesComparacaoPorDreQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues()
        {
            var query = new ObterUesComparacaoPorDreQuery(1, 2023, 10, 5);

            var esperado = new List<UePorDreDto>
            {
                new UePorDreDto { UeId = 100, UeNome = "Escola A" }
            };

            repositorio
                .Setup(r => r.ObterUesComparacaoPorDre(query.DreId, query.AnoAplicacao, query.DisciplinaId, query.AnoEscolar))
                .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(esperado, resultado);

            repositorio.Verify(r =>
                r.ObterUesComparacaoPorDre(query.DreId, query.AnoAplicacao, query.DisciplinaId, query.AnoEscolar),
                Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia()
        {
            var query = new ObterUesComparacaoPorDreQuery(1, 2023, 10, 5);

            repositorio
                .Setup(r => r.ObterUesComparacaoPorDre(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<UePorDreDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao()
        {
            var query = new ObterUesComparacaoPorDreQuery(1, 2023, 10, 5);

            repositorio
                .Setup(r => r.ObterUesComparacaoPorDre(query.DreId, query.AnoAplicacao, query.DisciplinaId, query.AnoEscolar))
                .ThrowsAsync(new System.Exception("Erro no repositório"));

            var exception = await Assert.ThrowsAsync<System.Exception>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", exception.Message);
        }
    }
}

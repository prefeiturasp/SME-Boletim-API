using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterTurmasUeAnoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterTurmasUeAnoQueryHandler handler;

        public ObterTurmasUeAnoQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterTurmasUeAnoQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Turmas()
        {
            var query = new ObterTurmasUeAnoQuery(10, 20, 30, 5);

            var esperado = new List<TurmaAnoDto>
            {
                new TurmaAnoDto { Turma = "A", Ano = 5, Descricao = "51" }
            };

            repositorio
                .Setup(r => r.ObterTurmasUeAno(query.LoteId, query.UeId, query.DisciplinaId, query.AnoEscolar))
                .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(esperado, resultado);

            repositorio.Verify(r =>
                r.ObterTurmasUeAno(query.LoteId, query.UeId, query.DisciplinaId, query.AnoEscolar),
                Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia()
        {
            var query = new ObterTurmasUeAnoQuery(10, 20, 30, 5);

            repositorio
                .Setup(r => r.ObterTurmasUeAno(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<TurmaAnoDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao()
        {
            var query = new ObterTurmasUeAnoQuery(10, 20, 30, 5);

            repositorio
                .Setup(r => r.ObterTurmasUeAno(query.LoteId, query.UeId, query.DisciplinaId, query.AnoEscolar))
                .ThrowsAsync(new System.Exception("Erro no repositório"));

            var exception = await Assert.ThrowsAsync<System.Exception>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", exception.Message);
        }
    }
}

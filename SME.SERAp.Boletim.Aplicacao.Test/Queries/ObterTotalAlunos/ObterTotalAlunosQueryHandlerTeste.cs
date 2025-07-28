using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTotalAlunosQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterTotalAlunosQueryHandler queryHandler;

        public ObterTotalAlunosQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterTotalAlunosQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Alunos()
        {
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();
            var total = 10;

            var query = new ObterTotalAlunosQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterTotalAlunos(loteId, anoEscolar)).ReturnsAsync(total);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);
            
            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalAlunos(loteId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Alunos_Zero()
        {
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();
            var total = 0;

            var query = new ObterTotalAlunosQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterTotalAlunos(loteId, anoEscolar)).ReturnsAsync(total);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalAlunos(loteId, anoEscolar), Times.Once);
        }
    }
}

using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTotalUesQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterTotalUesQueryHandler queryHandler;

        public ObterTotalUesQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterTotalUesQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Ues()
        {
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();
            var total = 5;
            var query = new ObterTotalUesQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterTotalUes(loteId, anoEscolar)).ReturnsAsync(total);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);
            
            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalUes(loteId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Ues_Zero()
        {
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();
            var total = 0;
            var query = new ObterTotalUesQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterTotalUes(loteId, anoEscolar)).ReturnsAsync(total);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);
            
            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalUes(loteId, anoEscolar), Times.Once);
        }
    }
}

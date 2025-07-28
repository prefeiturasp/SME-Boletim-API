using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTotalDresQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterTotalDresQueryHandler queryHandler;
        public ObterTotalDresQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterTotalDresQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Dres()
        {
            var total = It.IsAny<int>();
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            repositorio.Setup(r => r.ObterTotalDres(loteId, anoEscolar))
                .ReturnsAsync(total);

            var query = new ObterTotalDresQuery(loteId, anoEscolar);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalDres(query.LoteId, query.AnoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Total_Dres_Zero()
        {
            var total = 0;
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            repositorio.Setup(r => r.ObterTotalDres(loteId, anoEscolar))
                .ReturnsAsync(total);

            var query = new ObterTotalDresQuery(loteId, anoEscolar);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Equal(total, resultado);
            repositorio.Verify(r => r.ObterTotalDres(query.LoteId, query.AnoEscolar), Times.Once);
        }
    }
}

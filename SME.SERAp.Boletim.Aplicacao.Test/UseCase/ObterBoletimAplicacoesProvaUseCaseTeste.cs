using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProva;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimAplicacoesProvaUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Lista_De_LoteProvaDto()
        {
            var lotesSimulados = new List<LoteProvaDto>
            {
                new LoteProvaDto { Id = 1, Nome = "Lote 1" },
                new LoteProvaDto { Id = 2, Nome = "Lote 2" }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterLotesProvaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(lotesSimulados);

            var useCase = new ObterBoletimAplicacoesProvaUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                lote => Assert.Equal("Lote 1", lote.Nome),
                lote => Assert.Equal("Lote 2", lote.Nome));
        }
    }
}

using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterDresUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDresUseCase useCase;

        public ObterDresUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDresUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de DreDto corretamente")]
        public async Task DeveRetornarListaDeDres()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 16L;
            var listaEsperada = new List<DreDto>
            {
                new DreDto { DreId = 1, DreNome = "DRE 1", DreNomeAbreviado = "D1" },
                new DreDto { DreId = 2, DreNome = "DRE 2", DreNomeAbreviado = "D2" }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterDreQuery>(q => q.AnoEscolar == anoEscolar && q.LoteId == loteId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await useCase.Executar(anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("DRE 1", resultado.First().DreNome);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando não houver DREs")]
        public async Task DeveRetornarListaVazia()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<DreDto>());

            // Act
            var resultado = await useCase.Executar(5, 1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact(DisplayName = "Deve retornar null quando mediator retornar null")]
        public async Task DeveRetornarNullSeMediatorRetornarNull()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<DreDto>)null);

            // Act
            var resultado = await useCase.Executar(5, 1);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção quando mediator lançar exceção")]
        public async Task DeveLancarExcecaoQuandoMediatorLancar()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDreQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro no MediatR"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(5, 1));
        }
    }
}
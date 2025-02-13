using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test
{
    public class ObterBoletimEscolarPorUeUseCaseTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterBoletimEscolarPorUeUseCase _useCase;

        public ObterBoletimEscolarPorUeUseCaseTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterBoletimEscolarPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarBoletins_QuandoExistirem()
        {
            // Arrange
            long ueId = 12345;
            var boletinsMock = new List<BoletimEscolar>
            {
                new BoletimEscolar { UeId = ueId, ProvaId = 1, ComponenteCurricular = "Matemática" },
                new BoletimEscolar { UeId = ueId, ProvaId = 2, ComponenteCurricular = "Português" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterBoletimEscolarPorUeQuery>(), default))
                .ReturnsAsync(boletinsMock);

            // Act
            var resultado = await _useCase.Executar(ueId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Matemática", resultado.First().ComponenteCurricular);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterBoletimEscolarPorUeQuery>(q => q.UeId == ueId), default), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoNaoExistiremBoletins()
        {
            // Arrange
            long ueId = 54321;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterBoletimEscolarPorUeQuery>(), default))
                .ReturnsAsync((IEnumerable<BoletimEscolar>)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(ueId));
            Assert.Equal($"Não foi possível localizar boletins para a UE {ueId}", exception.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterBoletimEscolarPorUeQuery>(q => q.UeId == ueId), default), Times.Once);
        }
    }
}
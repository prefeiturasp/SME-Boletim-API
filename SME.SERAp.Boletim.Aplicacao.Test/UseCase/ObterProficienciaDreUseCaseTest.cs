using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaDreUseCaseTest
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaDreUseCase useCase;

        public ObterProficienciaDreUseCaseTest()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProficienciaDreUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar a lista de proficiência agrupada em um DTO de resposta válido")]
        public async Task DeveRetornarListaDeProficienciaAgrupada()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 16L;
            var listaEsperada = ObterProficienciaDreCompletoMock();

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterProficienciaDreQuery>(q => q.AnoEscolar == anoEscolar && q.LoteId == loteId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await useCase.Executar(anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Equal("DRE BUTANTA", resultado.Itens.First().DreNome);
            Assert.Equal(2, resultado.Itens.First().Disciplinas.Count());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar um objeto de retorno com listas vazias quando o MediatR retornar dados vazios")]
        public async Task DeveRetornarListaVaziaQuandoMediatRRetornarVazio()
        {
            // Arrange
            var resultadoEsperado = new ProficienciaDreCompletoDto
            {
                TotalTipoDisciplina = 0,
                Itens = Enumerable.Empty<DreProficienciaDto>()
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await useCase.Executar(5, 1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar null quando o MediatR retornar null")]
        public async Task DeveRetornarNullQuandoMediatRRetornarNull()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProficienciaDreCompletoDto)null);

            // Act
            var resultado = await useCase.Executar(5, 1);

            // Assert
            Assert.Null(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar uma exceção quando o MediatR lançar uma exceção")]
        public async Task DeveLancarExcecaoQuandoMediatRLancar()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro simulado do MediatR"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(5, 1));
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve chamar o MediatR com uma única DreId")]
        public async Task DeveChamarMediatRComDreIdUnica()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 16L;
            var dreId = new List<long> { 1 };
            var resultadoEsperado = ObterProficienciaDreCompletoMock();

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterProficienciaDreQuery>(q => q.AnoEscolar == anoEscolar && q.LoteId == loteId && q.DreIds.Contains(dreId.First())), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            await useCase.Executar(anoEscolar, loteId, dreId);

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve chamar o MediatR com múltiplas DreIds")]
        public async Task DeveChamarMediatRComMultiplasDreIds()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 16L;
            var dreIds = new List<long> { 1, 2 };
            var resultadoEsperado = ObterProficienciaDreCompletoMock();

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterProficienciaDreQuery>(q => q.AnoEscolar == anoEscolar && q.LoteId == loteId && q.DreIds.SequenceEqual(dreIds)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            await useCase.Executar(anoEscolar, loteId, dreIds);

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private ProficienciaDreCompletoDto ObterProficienciaDreCompletoMock()
        {
            var dadosDreButanta = new List<DisciplinaProficienciaDetalheDto>
            {
                new DisciplinaProficienciaDetalheDto { Disciplina = "Língua portuguesa", MediaProficiencia = 193.31m, NivelProficiencia = "Básico" },
                new DisciplinaProficienciaDetalheDto { Disciplina = "Matemática", MediaProficiencia = 176.67m, NivelProficiencia = "Básico" }
            };

            var dadosDreItaquera = new List<DisciplinaProficienciaDetalheDto>
            {
                new DisciplinaProficienciaDetalheDto { Disciplina = "Matemática", MediaProficiencia = 180.00m, NivelProficiencia = "Básico" }
            };

            var itens = new List<DreProficienciaDto>
            {
                new DreProficienciaDto
                {
                    DreNome = "DRE BUTANTA",
                    AnoEscolar = 5,
                    Disciplinas = dadosDreButanta
                },
                new DreProficienciaDto
                {
                    DreNome = "DRE ITAQUERA",
                    AnoEscolar = 5,
                    Disciplinas = dadosDreItaquera
                }
            };

            return new ProficienciaDreCompletoDto
            {
                TotalTipoDisciplina = 2,
                Itens = itens
            };
        }
    }
}
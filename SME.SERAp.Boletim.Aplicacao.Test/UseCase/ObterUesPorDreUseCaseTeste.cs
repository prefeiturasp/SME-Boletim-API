﻿using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterUesPorDreUseCaseTeste
    {
        [Fact]
        public async Task ExecutarAsync_DeveRetornarListaDeUePorDreDto_QuandoMediatorRetornaDados()
        {
            // Arrange
            var dreId = 11L;
            var anoEscolar = 5;
            var loteId = 16L;

            var retornoEsperado = new List<UePorDreDto>
            {
                new UePorDreDto
                {
                    UeId = 1001,
                    UeNome = "EMEF Teste 1",
                    TipoEscola = Dominio.Enumerados.TipoEscola.EMEF,
                    DreNomeAbreviado = "DRE LESTE",
                    DreId = dreId,
                    DreNome = "DIRETORIA REGIONAL DE EDUCACAO LESTE"
                },
                new UePorDreDto
                {
                    UeId = 1002,
                    UeNome = "EMEF Teste 2",
                    TipoEscola = Dominio.Enumerados.TipoEscola.EMEF,
                    DreNomeAbreviado = "DRE LESTE",
                    DreId = dreId,
                    DreNome = "DIRETORIA REGIONAL DE EDUCACAO LESTE"
                }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUesPorDreQuery>(
                        q => q.DreId == dreId && q.AnoEscolar == anoEscolar && q.LoteId == loteId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dreId, anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<UePorDreDto>)resultado).Count);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1001, item.UeId);
                    Assert.Equal("EMEF Teste 1", item.UeNome);
                },
                item =>
                {
                    Assert.Equal(1002, item.UeId);
                    Assert.Equal("EMEF Teste 2", item.UeNome);
                });
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarListaVazia_QuandoMediatorRetornaNada()
        {
            // Arrange
            var dreId = 20L;
            var anoEscolar = 3;
            var loteId = 30L;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UePorDreDto>());

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dreId, anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
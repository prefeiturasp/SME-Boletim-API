using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarPorUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_BoletimEscolarDto_Com_Dados_Esperados()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto();

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId, DreId = 1, UeNome = "Escola Teste" }
            };

            var boletinsOriginais = new List<BoletimEscolar>
            {
                new BoletimEscolar
                {
                    ProvaId = 10,
                    UeId = ueId,
                    ComponenteCurricular = "Matemática (5º Ano)",
                    AbaixoBasico = 10,
                    Basico = 20,
                    Adequado = 30,
                    Avancado = 40,
                    Total = 100,
                    MediaProficiencia = 75.5m,
                    AbaixoBasicoPorcentagem = 10m,
                    BasicoPorcentagem = 20m,
                    AdequadoPorcentagem = 30m,
                    AvancadoPorcentagem = 40m
                }
            };


            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangenciasUsuarioLogado);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterBoletimEscolarPorUeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(boletinsOriginais);

            var useCase = new ObterBoletimEscolarPorUeUseCase(mediatorMock.Object);
            var resultado = await useCase.Executar(loteId, ueId, filtros);

            var boletim = resultado.Single();

            Assert.Equal(10, boletim.ProvaId);
            Assert.Equal(ueId, boletim.UeId);
            Assert.Equal("Matemática (5º Ano)", boletim.ComponenteCurricular);
            Assert.Equal("10 (10%)", boletim.AbaixoBasico);
            Assert.Equal("20 (20%)", boletim.Basico);
            Assert.Equal("30 (30%)", boletim.Adequado);
            Assert.Equal("40 (40%)", boletim.Avancado);
            Assert.Equal(100, boletim.Total);
            Assert.Equal(75.5m, boletim.MediaProficiencia);
            Assert.Equal("Matemática", boletim.DisciplinaDescricao);
            Assert.Equal(5, boletim.AnoEscolar);
            Assert.Equal("5º Ano", boletim.AnoEscolarDescricao);
            Assert.Equal(2, boletim.DisciplinaId);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto();
            var abrangenciasVazias = new List<AbrangenciaUeDto>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangenciasVazias);

            var useCase = new ObterBoletimEscolarPorUeUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId, filtros));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Boletins_Nao_Sao_Encontrados()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto();

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId, DreId = 1, UeNome = "Escola Teste" }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangenciasUsuarioLogado);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterBoletimEscolarPorUeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<BoletimEscolar>)null);

            var useCase = new ObterBoletimEscolarPorUeUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(loteId, ueId, filtros));
            Assert.Equal($"Não foi possível localizar boletins para a UE {ueId}", ex.Message);
        }
    }
}

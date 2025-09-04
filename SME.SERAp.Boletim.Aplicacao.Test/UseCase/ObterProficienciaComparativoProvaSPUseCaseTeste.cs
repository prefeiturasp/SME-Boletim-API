using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaComparativoProvaSPUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoProvaSPUseCase useCase;

        public ObterProficienciaComparativoProvaSPUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProficienciaComparativoProvaSPUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_ProficienciaUeComparacaoProvaSPDto_Quando_Usuario_Tem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId }
            };

            var proficienciaComparacao = new ProficienciaUeComparacaoProvaSPDto
            {
                TotalLotes = 1,
                ProvaSP = new ProficienciaProvaSpDto
                {
                    LoteId = 0,
                    NomeAplicacao = "Prova SP",
                    MediaProficiencia = 550.0M,
                    NivelProficiencia = "Nível Adequado",
                    TotalRealizaramProva = 100
                },
                Lotes = new List<ProficienciaUeDto>
                {
                    new ProficienciaUeDto
                    {
                        LoteId = loteId,
                        NomeAplicacao = "Teste",
                        Periodo = "1 Semestre",
                        MediaProficiencia = 520.0M,
                        NivelProficiencia = "Nível Básico",
                        TotalRealizaramProva = 90
                    }
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangenciasUsuarioLogado);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(proficienciaComparacao);

            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.TotalLotes);
            Assert.Equal("Prova SP", resultado.ProvaSP.NomeAplicacao);
            Assert.Equal("Nível Adequado", resultado.ProvaSP.NivelProficiencia);
            Assert.Single(resultado.Lotes);
            Assert.Equal("Teste", resultado.Lotes.First().NomeAplicacao);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = 200 } // UE diferente
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangenciasUsuarioLogado);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId, disciplinaId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Abrangencias_Sao_Nulas()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<AbrangenciaUeDto>)null);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId, disciplinaId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterTurmasUeAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasUeAnoUseCase useCase;

        public ObterTurmasUeAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterTurmasUeAnoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Turmas_Quando_Usuario_Possui_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 10L;
            var disciplinaId = 5;
            var anoEscolar = 2025;

            var abrangencias = new List<AbrangenciaUeDto> { new AbrangenciaUeDto { UeId = ueId } };
            var turmasEsperadas = new List<TurmaAnoDto> { new TurmaAnoDto { Ano = 123, Descricao = "Turma A" } };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTurmasUeAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasEsperadas);

            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Turma A", resultado.First().Descricao);

            mediator.Verify(m => m.Send(It.IsAny<ObterTurmasUeAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Possui_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 20L; 
            var disciplinaId = 5;
            var anoEscolar = 2025;

            var abrangencias = new List<AbrangenciaUeDto> { new AbrangenciaUeDto { UeId = 10L } };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(loteId, ueId, disciplinaId, anoEscolar));

            mediator.Verify(m => m.Send(It.IsAny<ObterTurmasUeAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Abrangencias_Forem_Nulas()
        {
            var loteId = 1L;
            var ueId = 10L;
            var disciplinaId = 5;
            var anoEscolar = 2025;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AbrangenciaUeDto>)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(loteId, ueId, disciplinaId, anoEscolar));

            mediator.Verify(m => m.Send(It.IsAny<ObterTurmasUeAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

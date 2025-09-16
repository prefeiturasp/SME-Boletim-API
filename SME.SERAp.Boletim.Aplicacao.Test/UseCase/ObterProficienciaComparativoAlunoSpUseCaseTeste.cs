using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaComparativoAlunoSpUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly IObterProficienciaComparativoAlunoSpUseCase useCase;

        public ObterProficienciaComparativoAlunoSpUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProficienciaComparativoAlunoSpUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar ProficienciaComparativoAlunoSpDto quando o usuário tem abrangência")]
        public async Task Executar_Deve_Retornar_ProficienciaComparativoAlunoSpDto_Quando_Usuario_Tem_Abrangencia()
        {
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var turma = "Turma 1";
            var anoCriacao = 2024;
            var pagina = 1;
            var itensPorPagina = 10;

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId }
            };

            var proficienciaComparativo = new ProficienciaComparativoAlunoSpDto
            {
                Total = 5,
                Pagina = 1,
                ItensPorPagina = 10,
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>()
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasUsuarioLogado);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciaComparativo);

            var resultado = await useCase.Executar(ueId, disciplinaId, anoEscolar, turma, anoCriacao, pagina, itensPorPagina);

            Assert.NotNull(resultado);
            Assert.IsType<ProficienciaComparativoAlunoSpDto>(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar NaoAutorizadoException quando o usuário não tem abrangência")]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var turma = "Turma 1";
            var anoCriacao = 2024;
            var pagina = 1;
            var itensPorPagina = 10;

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = 200 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasUsuarioLogado);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(ueId, disciplinaId, anoEscolar, turma, anoCriacao, pagina, itensPorPagina));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar NaoAutorizadoException quando as abrangências são nulas")]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Abrangencias_Sao_Nulas()
        {
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var turma = "Turma 1";
            var anoCriacao = 2024;
            var pagina = 1;
            var itensPorPagina = 10;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AbrangenciaUeDto>)null);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(ueId, disciplinaId, anoEscolar, turma, anoCriacao, pagina, itensPorPagina));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
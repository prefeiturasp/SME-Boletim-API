using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDresComparativoSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDresComparativoSmeUseCase useCase;

        public ObterDresComparativoSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDresComparativoSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Comparativo_Sme_Quando_Usuario_For_Administrador()
        {
            var anoAplicacao = 2025;
            var disciplinaId = 5;
            var anoEscolar = 5;

            var dresEsperadas = new List<DreDto>
            {
                new DreDto { DreId = 1, DreNome = "DRE Teste 1" },
                new DreDto { DreId = 2, DreNome = "DRE Teste 2" }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator.Setup(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresEsperadas);

            var resultado = await useCase.Executar(anoAplicacao, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(dresEsperadas.Count, resultado.Count());
            Assert.Contains(resultado, x => x.DreNome == "DRE Teste 1");

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_For_Administrador()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(2025, 1, 3));

            Assert.Equal("Usuário sem permissão.", excecao.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Tipo_Perfil_For_Nulo()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(2025, 1, 3));

            Assert.Equal("Usuário sem permissão.", excecao.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterComponentesCurricularesSmePorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterComponentesCurricularesSmePorAnoUseCase useCase;

        public ObterComponentesCurricularesSmePorAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterComponentesCurricularesSmePorAnoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Componentes_Quando_Usuario_Eh_Administrador()
        {
            var anoAplicacao = 2025;
            var perfil = TipoPerfil.Administrador;
            var componentesEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 4, Texto = "Matemática" },
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "Português" }
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perfil);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesSmePorAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesEsperados);

            var resultado = await useCase.Executar(anoAplicacao);

            Assert.NotNull(resultado);
            Assert.Equal(componentesEsperados, resultado);

            mediator.Verify(m => m.Send(It.Is<ObterComponentesCurricularesSmePorAnoQuery>(
                q => q.AnoAplicacao == anoAplicacao
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Nulo()
        {
            var anoAplicacao = 2025;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(anoAplicacao));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Diferente_De_Administrador()
        {
            var anoAplicacao = 2025;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(anoAplicacao));
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Mediator_Falhar()
        {
            var anoAplicacao = 2025;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro no mediator"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.Executar(anoAplicacao));

            Assert.Equal("Erro no mediator", exception.Message);
        }
    }
}

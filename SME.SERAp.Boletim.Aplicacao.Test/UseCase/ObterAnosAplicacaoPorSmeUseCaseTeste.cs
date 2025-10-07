using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosAplicacaoPorSme;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterAnosAplicacaoPorSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAnosAplicacaoPorSmeUseCase useCase;

        public ObterAnosAplicacaoPorSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAnosAplicacaoPorSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Quando_O_UseCase_E_Executado()
        {
            var anosEsperados = new List<int> { 2021, 2022, 2023 };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosAplicacaoPorSmeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEsperados);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(anosEsperados, resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosAplicacaoPorSmeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Ha_Anos_De_Aplicacao()
        {
            var anosEsperados = new List<int>();

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosAplicacaoPorSmeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEsperados);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosAplicacaoPorSmeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Nao_Autorizado_Quando_Usuario_Nao_For_Administrador()
        {
            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar());
            Assert.Equal("Usuário sem permissão.", ex.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosAplicacaoPorSmeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
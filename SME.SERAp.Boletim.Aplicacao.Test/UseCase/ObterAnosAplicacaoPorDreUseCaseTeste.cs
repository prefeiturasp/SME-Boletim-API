using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterAnosAplicacaoPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAnosAplicacaoPorDreUseCase useCase;

        public ObterAnosAplicacaoPorDreUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAnosAplicacaoPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Quando_Usuario_Tem_Abrangencia_E_Permissao()
        {
            var dreId = 10L;
            var dres = new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } };
            var perfil = TipoPerfil.Administrador;
            var anosEsperados = new List<int> { 2021, 2022, 2023 };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perfil);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosAplicacaoPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEsperados);

            var resultado = await useCase.Executar(dreId);

            Assert.NotNull(resultado);
            Assert.Equal(anosEsperados, resultado);

            mediator.Verify(m => m.Send(It.Is<ObterAnosAplicacaoPorDreQuery>(
                q => q.DreId == dreId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var dreId = 10L;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto>());

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Tipo_Perfil_Nulo()
        {
            var dreId = 10L;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Sem_Permissao()
        {
            var dreId = 10L;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId));
        }
    }
}

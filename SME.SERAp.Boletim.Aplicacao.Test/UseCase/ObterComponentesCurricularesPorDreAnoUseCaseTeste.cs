using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterComponentesCurricularesPorDreAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterComponentesCurricularesPorDreAnoUseCase useCase;

        public ObterComponentesCurricularesPorDreAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterComponentesCurricularesPorDreAnoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Componentes_Quando_Usuario_Tem_Abrangencia_E_Permissao()
        {
            var dreId = 10L;
            var anoAplicacao = 2022;
            var dres = new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } };
            var perfil = TipoPerfil.Administrador;
            var componentesEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> {Valor = 4, Texto = "Matemática"},
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "Português" }
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perfil);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorDreAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesEsperados);

            var resultado = await useCase.Executar(dreId, anoAplicacao);

            Assert.NotNull(resultado);
            Assert.Equal(componentesEsperados, resultado);

            mediator.Verify(m => m.Send(It.Is<ObterComponentesCurricularesPorDreAnoQuery>(
                q => q.DreId == dreId && q.AnoAplicacao == anoAplicacao
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var dreId = 10L;
            var anoAplicacao = 2022;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto>());

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId, anoAplicacao));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Tipo_Perfil_Nulo()
        {
            var dreId = 10L;
            var anoAplicacao = 2022;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId, anoAplicacao));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Sem_Permissao()
        {
            var dreId = 10L;
            var anoAplicacao = 2022;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(dreId, anoAplicacao));
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Mediator_Falhar()
        {
            var dreId = 10L;
            var anoAplicacao = 2022;

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro no mediator"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.Executar(dreId, anoAplicacao));

            Assert.Equal("Erro no mediator", exception.Message);
        }
    }
}

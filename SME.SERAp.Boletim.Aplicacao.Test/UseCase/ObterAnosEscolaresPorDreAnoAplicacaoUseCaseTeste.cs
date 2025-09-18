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
    public class ObterAnosEscolaresPorDreAnoAplicacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAnosEscolaresPorDreAnoAplicacaoUseCase useCase;

        public ObterAnosEscolaresPorDreAnoAplicacaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAnosEscolaresPorDreAnoAplicacaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Escolares_Quando_Usuario_Tem_Abrangencia_E_Permissao()
        {
            var dreId = 10L;
            var anoAplicacao = 2023;
            var disciplinaId = 5;
            var dres = new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } };
            var perfil = TipoPerfil.Administrador;

            var anosEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> {Valor = 1, Texto = "1"},
                new OpcaoFiltroDto<int> {Valor = 2, Texto = "2"}
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perfil);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorDreAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEsperados);

            var resultado = await useCase.Executar(dreId, anoAplicacao, disciplinaId);

            Assert.NotNull(resultado);
            Assert.Equal(anosEsperados, resultado);

            mediator.Verify(m => m.Send(It.Is<ObterAnosEscolaresPorDreAnoAplicacaoQuery>(
                q => q.DreId == dreId && q.AnoAplicacao == anoAplicacao && q.DisciplinaId == disciplinaId
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
                useCase.Executar(dreId, 2023, 5));
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
                useCase.Executar(dreId, 2023, 5));
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
                useCase.Executar(dreId, 2023, 5));
        }
    }
}

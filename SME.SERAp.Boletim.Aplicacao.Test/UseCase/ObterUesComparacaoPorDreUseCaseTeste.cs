using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterUesComparacaoPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterUesComparacaoPorDreUseCase useCase;

        public ObterUesComparacaoPorDreUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterUesComparacaoPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Quando_Usuario_Tem_Abrangencia_E_Permissao()
        {
            var dreId = 10L;
            var anoAplicacao = 2023;
            var disciplinaId = 5;
            var anoEscolar = 9;
            var dres = new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } };
            var perfil = TipoPerfil.Administrador;
            var uesEsperadas = new List<UePorDreDto>
            {
                new UePorDreDto { UeId = 100, UeNome = "Escola A" },
                new UePorDreDto { UeId = 200, UeNome = "Escola B" },
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perfil);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterUesComparacaoPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesEsperadas);

            var resultado = await useCase.Executar(dreId, anoAplicacao, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(uesEsperadas.Count, resultado.Count());
            Assert.Equal(uesEsperadas, resultado);

            mediator.Verify(m => m.Send(It.Is<ObterUesComparacaoPorDreQuery>(
                q => q.DreId == dreId &&
                     q.AnoAplicacao == anoAplicacao &&
                     q.DisciplinaId == disciplinaId &&
                     q.AnoEscolar == anoEscolar
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
                useCase.Executar(dreId, 2023, 5, 9));
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
                useCase.Executar(dreId, 2023, 5, 9));
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
                useCase.Executar(dreId, 2023, 5, 9));
        }
    }
}

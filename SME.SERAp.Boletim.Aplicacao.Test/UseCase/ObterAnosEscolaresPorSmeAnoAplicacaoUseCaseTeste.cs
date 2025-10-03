using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosEscolaresPorSmeAnoAplicacao;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterAnosEscolaresPorSmeAnoAplicacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAnosEscolaresPorSmeAnoAplicacaoUseCase useCase;

        public ObterAnosEscolaresPorSmeAnoAplicacaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAnosEscolaresPorSmeAnoAplicacaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Escolares_Quando_Usuario_For_Administrador()
        {
            var anoAplicacao = 2023;
            var disciplinaId = 1;
            var anosEscolaresEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 1, Texto = "1º Ano" }
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorSmeAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEscolaresEsperados);

            var resultado = await useCase.Executar(anoAplicacao, disciplinaId);

            Assert.NotNull(resultado);
            Assert.Equal(anosEscolaresEsperados, resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorSmeAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Ha_Anos_De_Aplicacao()
        {
            var anoAplicacao = 2023;
            var disciplinaId = 1;
            var anosEscolaresEsperados = new List<OpcaoFiltroDto<int>>();

            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorSmeAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEscolaresEsperados);

            var resultado = await useCase.Executar(anoAplicacao, disciplinaId);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorSmeAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Nao_Autorizado_Quando_Usuario_Nao_For_Administrador()
        {
            var anoAplicacao = 2023;
            var disciplinaId = 1;
            mediator
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(anoAplicacao, disciplinaId));
            Assert.Equal("Usuário sem permissão.", ex.Message);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorSmeAnoAplicacaoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
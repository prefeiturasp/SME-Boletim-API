using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDresAbrangenciaUsuarioLogadoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDresAbrangenciaUsuarioLogadoUseCase obterDresAbrangenciaUsuarioLogadoUseCase;
        public ObterDresAbrangenciaUsuarioLogadoUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.obterDresAbrangenciaUsuarioLogadoUseCase = new ObterDresAbrangenciaUsuarioLogadoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_De_Abrangencias_Do_Usuario_Autenticado()
        {
            var dresEsperadas = new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 1 },
                new DreAbragenciaDetalheDto { Id = 2 }
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresEsperadas);

            var resultado = await obterDresAbrangenciaUsuarioLogadoUseCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, a => a.Id == 1);
            Assert.Contains(resultado, a => a.Id == 2);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dres_De_Abrangencias_Do_Usuario_Autenticado()
        {
            mediator
                .Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto>());

            var resultado = await obterDresAbrangenciaUsuarioLogadoUseCase.Executar();

            Assert.Empty(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

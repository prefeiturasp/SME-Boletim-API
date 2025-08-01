using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDownloadSmeResultadoProbabilidadeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDownloadSmeResultadoProbabilidadeUseCase useCase;
        public ObterDownloadSmeResultadoProbabilidadeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDownloadSmeResultadoProbabilidadeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Resultado_Probabilidade_Sme()
        {
            var loteId = 1L;
            var tipoPerfil = TipoPerfil.Administrador;
            var itens = ObterBoletimProvas();

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);
            mediator.Setup(m => m.Send(It.IsAny<ObterDownloadSmeResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(itens);

            var result = await useCase.Executar(loteId);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDownloadSmeResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Sem_Dados_Resultado_Probabilidade_Sme()
        {
            var loteId = 1L;
            var tipoPerfil = TipoPerfil.Administrador;

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            mediator.Setup(m => m.Send(It.IsAny<ObterDownloadSmeResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DownloadResultadoProbabilidadeDto>());

            var result = await useCase.Executar(loteId);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDownloadSmeResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Excecao_Usuario_Sem_Permissao()
        {
            var loteId = 1L;
            var tipoPerfil = TipoPerfil.Administrador_DRE;
            var itens = ObterBoletimProvas();

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId));
            Assert.Equal("Usuário sem permissão.", excecao.Message);
        }

        private static List<DownloadResultadoProbabilidadeDto> ObterBoletimProvas()
        {
            return new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto
                {
                    Componente = "Matemática",
                    NomeDreAbreviacao = "DRE Teste",
                    AbaixoDoBasico = 10,
                    Adequado = 20,
                    Avancado = 30,
                    AnoEscolar = 5,
                    Basico = 40,
                    CodigoDre = "DRE123",
                    CodigoHabilidade = "HAB123",
                    CodigoUe = "UE123",
                    HabilidadeDescricao = "Habilidade Teste",
                    NomeUe = "Escola Teste",
                    TurmaDescricao = "Turma A"
                },
                new DownloadResultadoProbabilidadeDto
                {
                    Componente = "Matemática",
                    NomeDreAbreviacao = "DRE Teste",
                    AbaixoDoBasico = 40,
                    Adequado = 50,
                    Avancado = 60,
                    AnoEscolar = 5,
                    Basico = 70,
                    CodigoDre = "DRE123",
                    CodigoHabilidade = "HAB124",
                    CodigoUe = "UE123",
                    HabilidadeDescricao = "Habilidade Teste 2",
                    NomeUe = "Escola Teste",
                    TurmaDescricao = "Turma A"
                }
            };
        }
    }
}

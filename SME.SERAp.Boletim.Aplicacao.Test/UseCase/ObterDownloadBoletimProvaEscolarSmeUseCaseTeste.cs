using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDownloadBoletimProvaEscolarSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDownloadBoletimProvaEscolarSmeUseCase useCase;
        public ObterDownloadBoletimProvaEscolarSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDownloadBoletimProvaEscolarSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Provas_Boletim_Escolar_Sme()
        {
            var loteId = 1L;
            var tipoPerfil = TipoPerfil.Administrador;
            var itens = ObterBoletimProvas();

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);
            mediator.Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarSmeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(itens);

            var result = await useCase.Executar(loteId);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarSmeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Sem_Dados_Provas_Boletim_Escolar_Sme()
        {
            var loteId = 1L;
            var tipoPerfil = TipoPerfil.Administrador;

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            mediator.Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarSmeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DownloadProvasBoletimEscolarPorDreDto>());

            var result = await useCase.Executar(loteId);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarSmeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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

        private static List<DownloadProvasBoletimEscolarPorDreDto> ObterBoletimProvas()
        {
            return new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto
                {
                    NomeDreAbreviacao = "DRE Teste",
                    CodigoUE = "123456",
                    NomeUE = "Escola Teste",
                    AnoEscola = 2023,
                    Turma = "Turma A",
                    AlunoRA = 123,
                    NomeAluno = "Aluno Teste",
                    Componente = "Matemática",
                    Proficiencia = 75.5M,
                    Nivel = "Abaixo básico"
                },
                new DownloadProvasBoletimEscolarPorDreDto
                {
                    NomeDreAbreviacao = "DRE Teste",
                    CodigoUE = "123456",
                    NomeUE = "Escola Teste",
                    AnoEscola = 2023,
                    Turma = "Turma A",
                    AlunoRA = 456,
                    NomeAluno = "Aluno Teste 2",
                    Componente = "Matemática",
                    Proficiencia = 123.5M,
                    Nivel = "Abaixo básico"
                }
            };
        }
    }
}

using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadDreResultadoProbabilidade;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDownloadDreResultadoProbabilidadeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDownloadDreResultadoProbabilidadeUseCase useCase;

        public ObterDownloadDreResultadoProbabilidadeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDownloadDreResultadoProbabilidadeUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve gerar o download com o conteúdo HTML e dados corretos")]
        public async Task Deve_Gerar_Download_Com_Conteudo_HTML_E_Dados_Corretos()
        {
            var loteId = 1L;
            var dreId = 1;
            var tipoPerfil = TipoPerfil.Administrador;
            var dadosDeTeste = ObterDadosDeTeste();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadDreResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosDeTeste);

            var resultado = await useCase.Executar(loteId, dreId);

            Assert.NotNull(resultado);
            Assert.True(resultado.Length > 0);

            using var reader = new StreamReader(resultado, Encoding.UTF8);
            var conteudo = await reader.ReadToEndAsync();

            Assert.Contains("<html>", conteudo);
            Assert.Contains("<table border='1'>", conteudo);
            Assert.Contains("<th>Componente</th>", conteudo);
            Assert.Contains("<th>Nome DRE</th>", conteudo);
            Assert.Contains("<td>Língua Portuguesa</td>", conteudo);
            Assert.Contains("<td>DRE A</td>", conteudo);
            Assert.Contains("<td class=\"numero\">UE1</td>", conteudo);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDownloadDreResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve gerar o download com apenas o cabeçalho quando não há dados")]
        public async Task Deve_Gerar_Download_Com_Apenas_Cabecalho_Quando_Nao_Ha_Dados()
        {
            var loteId = 1L;
            var dreId = 1;
            var tipoPerfil = TipoPerfil.Administrador;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadDreResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DownloadResultadoProbabilidadeDto>());

            var resultado = await useCase.Executar(loteId, dreId);

            Assert.NotNull(resultado);
            Assert.True(resultado.Length > 0);

            using var reader = new StreamReader(resultado, Encoding.UTF8);
            var conteudo = await reader.ReadToEndAsync();

            Assert.Contains("<table border='1'>", conteudo);
            Assert.Contains("<tr><th>Componente</th><th>Nome DRE</th><th>Codigo UE</th><th>Nome UE</th><th>Ano Escola</th><th>Turma</th><th>Código Habilidade</th><th>Habilidade</th><th>Abaixo do Básico</th><th>Básico</th><th>Adequado</th><th>Avançado</th></tr>", conteudo);
            Assert.DoesNotContain("<td>Língua Portuguesa</td>", conteudo);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDownloadDreResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar exceção quando o usuário não possuir permissão")]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Possuir_Permissao()
        {
            var loteId = 1L;
            var dreId = 1;
            var tipoPerfil = TipoPerfil.Professor;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfil);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, dreId));
            Assert.Equal("Usuário sem permissão.", excecao.Message);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDownloadDreResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static List<DownloadResultadoProbabilidadeDto> ObterDadosDeTeste()
        {
            return new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto
                {
                    Componente = "Língua Portuguesa",
                    NomeDreAbreviacao = "DRE A",
                    AbaixoDoBasico = 10,
                    Adequado = 20,
                    Avancado = 30,
                    AnoEscolar = 5,
                    Basico = 40,
                    CodigoDre = "DRE123",
                    CodigoHabilidade = "HAB1",
                    CodigoUe = "UE1",
                    HabilidadeDescricao = "Habilidade 1",
                    NomeUe = "Escola 1",
                    TurmaDescricao = "Turma 5A"
                },
                new DownloadResultadoProbabilidadeDto
                {
                    Componente = "Matemática",
                    NomeDreAbreviacao = "DRE A",
                    AbaixoDoBasico = 15,
                    Adequado = 25,
                    Avancado = 35,
                    AnoEscolar = 5,
                    Basico = 45,
                    CodigoDre = "DRE123",
                    CodigoHabilidade = "HAB2",
                    CodigoUe = "UE2",
                    HabilidadeDescricao = "Habilidade 2",
                    NomeUe = "Escola 2",
                    TurmaDescricao = "Turma 5B"
                }
            };
        }
    }
}
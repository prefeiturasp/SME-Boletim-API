using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolarPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Globalization;
using System.Text;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterDownloadBoletimProvaEscolarPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDownloadBoletimProvaEscolarPorDreUseCase useCase;

        public ObterDownloadBoletimProvaEscolarPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDownloadBoletimProvaEscolarPorDreUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar arquivo corretamente com dados válidos")]
        public async Task DeveRetornarArquivoComDadosValidos()
        {
            // Arrange
            var dados = new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto
                {
                    NomeDreAbreviacao = "DRE Teste",
                    CodigoUE = "123",
                    NomeUE = "Escola Teste",
                    AnoEscola = 5,
                    Turma = "5A",
                    AlunoRA = 123456789,
                    NomeAluno = "João da Silva",
                    Componente = "Matemática",
                    Proficiencia = 250.5m,
                    Nivel = "Básico"
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            // Act
            var result = await useCase.Executar(1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);

            result.Position = 0;
            using var reader = new StreamReader(result, Encoding.UTF8);
            var content = await reader.ReadToEndAsync();

            content.Should().Contain("<th>Nome DRE</th>");
            content.Should().Contain("<th>Codigo UE</th>");
            content.Should().Contain("<th>Nome UE</th>");
            content.Should().Contain("<th>Ano Escola</th>");
            content.Should().Contain("<th>Turma</th>");
            content.Should().Contain("<th>Aluno RA</th>");
            content.Should().Contain("<th>Nome Aluno</th>");
            content.Should().Contain("<th>Componente</th>");
            content.Should().Contain("<th>Proficiência</th>");
            content.Should().Contain("<th>Nível</th>");

            content.Should().Contain("DRE Teste");
            content.Should().Contain("123");
            content.Should().Contain("Escola Teste");
            content.Should().Contain("5A");
            content.Should().Contain("João da Silva");
            content.Should().Contain("Matemática");
            content.Should().Contain(250.5.ToString("N1", new CultureInfo("pt-BR")));
            content.Should().Contain("Básico");
        }

        [Fact(DisplayName = "Deve retornar arquivo com apenas o cabeçalho se não houver dados")]
        public async Task DeveRetornarArquivoComCabecalhoSeSemDados()
        {
            // Arrange
            var dados = new List<DownloadProvasBoletimEscolarPorDreDto>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            // Act
            var result = await useCase.Executar(10, 20);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);

            result.Position = 0;
            using var reader = new StreamReader(result, Encoding.UTF8);
            var content = await reader.ReadToEndAsync();

            content.Should().Contain("<table border='1'>");
            content.Should().Contain("<th>Codigo UE</th>");
            content.Should().NotContain("<td>");
        }

        [Fact(DisplayName = "Deve lançar exceção se ocorrer erro ao buscar dados")]
        public async Task DeveLancarExcecaoAoBuscarDados()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao buscar dados"));

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(1, 10));
        }

        [Fact(DisplayName = "Deve chamar mediator.Send com parâmetros corretos")]
        public async Task DeveChamarMediatorComParametrosCorretos()
        {
            // Arrange
            var dados = new List<DownloadProvasBoletimEscolarPorDreDto>();
            long loteId = 3;
            long dreId = 10;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            // Act
            await useCase.Executar(loteId, dreId);

            // Assert
            mediatorMock.Verify(m => m.Send(It.Is<ObterDownloadProvasBoletimEscolarPorDreQuery>(
                x => x.LoteId == loteId && x.DreId == dreId), It.IsAny<CancellationToken>()), Times.Once);
        }

        private IEnumerable<DreAbragenciaDetalheDto> ObterDresAbrangenciaUsuarioLogado()
        {
            return new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 10, Abreviacao = "DT1", Codigo = "111", Nome = "Dre teste 1"},
                new DreAbragenciaDetalheDto { Id = 20, Abreviacao = "DT2", Codigo = "112", Nome = "Dre teste 2"}
            };
        }
    }
}
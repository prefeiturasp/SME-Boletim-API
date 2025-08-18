using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterDownloadProvasBoletimEscolar
{
    public class ObterDownloadProvasBoletimEscolarQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_DownloadProvas_Quando_Existir()
        {
            // Arrange
            var loteId = 1L;
            var ueId = 123L;
            var provasEsperadas = new List<DownloadProvasBoletimEscolarDto>
            {
                new DownloadProvasBoletimEscolarDto
                {
                    CodigoUE = "123",
                    NomeUE = "Escola Teste",
                    AnoEscola = 2024,
                    Turma = "A",
                    AlunoRA = 456789,
                    NomeAluno = "João",
                    Componente = "Matemática",
                    Proficiencia = 250.5m,
                    Nivel = "Adequado"
                }
            };

            var repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            repositorioBoletimEscolarMock
                .Setup(r => r.ObterDownloadProvasBoletimEscolar(loteId, ueId))
                .ReturnsAsync(provasEsperadas);

            var handler = new ObterDownloadProvasBoletimEscolarQueryHandler(repositorioBoletimEscolarMock.Object);
            var query = new ObterDownloadProvasBoletimEscolarQuery(loteId, ueId);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            var prova = ((List<DownloadProvasBoletimEscolarDto>)resultado)[0];
            Assert.Equal("123", prova.CodigoUE);
            Assert.Equal("Escola Teste", prova.NomeUE);
            Assert.Equal(2024, prova.AnoEscola);
            Assert.Equal("A", prova.Turma);
            Assert.Equal(456789, prova.AlunoRA);
            Assert.Equal("João", prova.NomeAluno);
            Assert.Equal("Matemática", prova.Componente);
            Assert.Equal(250.5m, prova.Proficiencia);
            Assert.Equal("Adequado", prova.Nivel);
        }

        [Fact]
        public async Task Deve_Retornar_Nulo_Quando_Nao_Existir_DownloadProvas()
        {
            // Arrange
            var loteId = 1L;
            var ueId = 999L;

            var repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            repositorioBoletimEscolarMock
                .Setup(r => r.ObterDownloadProvasBoletimEscolar(loteId, ueId))
                .ReturnsAsync((IEnumerable<DownloadProvasBoletimEscolarDto>)null);

            var handler = new ObterDownloadProvasBoletimEscolarQueryHandler(repositorioBoletimEscolarMock.Object);
            var query = new ObterDownloadProvasBoletimEscolarQuery(loteId, ueId);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }
    }
}
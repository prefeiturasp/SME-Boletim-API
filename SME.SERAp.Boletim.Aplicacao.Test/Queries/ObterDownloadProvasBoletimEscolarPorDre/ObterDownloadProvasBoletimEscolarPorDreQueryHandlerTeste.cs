using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolarPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterDownloadProvasBoletimEscolarPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterDownloadProvasBoletimEscolarPorDreQueryHandler handler;

        public ObterDownloadProvasBoletimEscolarPorDreQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterDownloadProvasBoletimEscolarPorDreQueryHandler(repositorioMock.Object);
        }

        [Fact(DisplayName = "Deve chamar o repositório com os parâmetros corretos e retornar os DTOs")]
        public async Task Handle_Deve_ChamarRepositorio_E_RetornarDtos()
        {
            var dreId = 1L;
            var loteId = 2L;

            var listaDto = new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto
                {
                    CodigoDre = dreId,
                    NomeDreAbreviacao = "DRE1",
                    CodigoUE = "UE123",
                    NomeUE = "Escola Teste",
                    AnoEscola = 9,
                    Turma = "9A",
                    AlunoRA = 123456,
                    NomeAluno = "Aluno Teste",
                    Componente = "Matemática",
                    Proficiencia = 7.5m,
                    Nivel = "B"
                }
            };

            repositorioMock
                .Setup(r => r.ObterDownloadProvasBoletimEscolarPorDre(dreId, loteId))
                .ReturnsAsync(listaDto);

            var query = new ObterDownloadProvasBoletimEscolarPorDreQuery(dreId, loteId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.IsAssignableFrom<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>>(resultado);
            Assert.Single(resultado);
            Assert.Equal(dreId, resultado.First().CodigoDre);

            repositorioMock.Verify(r => r.ObterDownloadProvasBoletimEscolarPorDre(dreId, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando repositório não retornar dados")]
        public async Task Handle_Deve_RetornarListaVazia_QuandoRepositorioNaoRetornarDados()
        {
            var dreId = 1L;
            var loteId = 2L;

            repositorioMock
                .Setup(r => r.ObterDownloadProvasBoletimEscolarPorDre(dreId, loteId))
                .ReturnsAsync(new List<DownloadProvasBoletimEscolarPorDreDto>());

            var query = new ObterDownloadProvasBoletimEscolarPorDreQuery(dreId, loteId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioMock.Verify(r => r.ObterDownloadProvasBoletimEscolarPorDre(dreId, loteId), Times.Once);
        }
    }
}
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadResultadoProbabilidade;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDownloadResultadoProbabilidadeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterDownloadResultadoProbabilidadeQueryHandler queryHandler;

        public ObterDownloadResultadoProbabilidadeQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDownloadResultadoProbabilidadeQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Probabilidade()
        {
            var loteId = 1L;
            var ueId = 10;
            var disciplinaId = 20;
            var anoEscolar = 2023;

            var itens = new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto
                {
                    NomeDreAbreviacao = "DRE Teste",
                    CodigoUe = "123456",
                    NomeUe = "Escola Teste",
                    AnoEscolar = anoEscolar,
                    TurmaDescricao = "Turma A",
                    AbaixoDoBasico = 10.5m,
                    Basico = 20.0m,
                    Adequado = 30.0m,
                    Avancado = 39.5m,
                    Componente = "Matemática",
                    CodigoHabilidade = "H123",
                    CodigoDre = "D123",
                    HabilidadeDescricao = "Habilidade de teste"
                }
            };

            repositorio.Setup(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar))
                       .ReturnsAsync(itens);

            var query = new ObterDownloadResultadoProbabilidadeQuery(loteId, ueId, disciplinaId, anoEscolar);

            var result = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(itens.Count, result.Count());
            repositorio.Verify(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Resultados_Probabilidade()
        {
            var loteId = 1L;
            var ueId = 10;
            var disciplinaId = 20;
            var anoEscolar = 2023;

            repositorio.Setup(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar))
                       .ReturnsAsync(new List<DownloadResultadoProbabilidadeDto>());

            var query = new ObterDownloadResultadoProbabilidadeQuery(loteId, ueId, disciplinaId, anoEscolar);

            var result = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
            repositorio.Verify(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Repositorio_Falhar()
        {
            var loteId = 1L;
            var ueId = 10;
            var disciplinaId = 20;
            var anoEscolar = 2023;

            repositorio.Setup(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar))
                       .ThrowsAsync(new Exception("Erro no repositório"));

            var query = new ObterDownloadResultadoProbabilidadeQuery(loteId, ueId, disciplinaId, anoEscolar);

            await Assert.ThrowsAsync<Exception>(() => queryHandler.Handle(query, CancellationToken.None));
            repositorio.Verify(r => r.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar), Times.Once);
        }
    }
}

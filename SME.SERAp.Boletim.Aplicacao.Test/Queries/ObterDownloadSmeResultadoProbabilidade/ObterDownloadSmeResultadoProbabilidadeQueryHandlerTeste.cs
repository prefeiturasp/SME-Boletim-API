using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDownloadSmeResultadoProbabilidadeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterDownloadSmeResultadoProbabilidadeQueryHandler queryHandler;
        public ObterDownloadSmeResultadoProbabilidadeQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDownloadSmeResultadoProbabilidadeQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultado_Probabilidade_Sme()
        {
            var loteId = 1L;
            var itens = new List<DownloadResultadoProbabilidadeDto>
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

            repositorio.Setup(r => r.ObterDownloadSmeResultadoProbabilidade(loteId))
                .ReturnsAsync(itens);

            var result = await queryHandler.Handle(new ObterDownloadSmeResultadoProbabilidadeQuery(loteId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(itens.Count, result.Count());
            repositorio.Verify(r => r.ObterDownloadSmeResultadoProbabilidade(loteId), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Resultado_Probabilidade_Sme()
        {
            var loteId = 1L;

            repositorio.Setup(r => r.ObterDownloadSmeResultadoProbabilidade(loteId))
                .ReturnsAsync(new List<DownloadResultadoProbabilidadeDto>());

            var result = await queryHandler.Handle(new ObterDownloadSmeResultadoProbabilidadeQuery(loteId), CancellationToken.None);

            Assert.Empty(result);
            repositorio.Verify(r => r.ObterDownloadSmeResultadoProbabilidade(loteId), Times.Once);
        }
    }
}

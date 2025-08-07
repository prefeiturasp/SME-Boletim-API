using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadDreResultadoProbabilidade;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterDownloadDreResultadoProbabilidade
{
    public class ObterDownloadDreResultadoProbabilidadeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterDownloadDreResultadoProbabilidadeQueryHandler queryHandler;

        public ObterDownloadDreResultadoProbabilidadeQueryHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDownloadDreResultadoProbabilidadeQueryHandler(repositorioMock.Object);
        }

        [Fact(DisplayName = "Deve retornar a lista de resultados de probabilidade da DRE")]
        public async Task Deve_Retornar_Resultado_Probabilidade_Dre()
        {
            var loteId = 1L;
            var dreId = 1;

            var itens = new List<DownloadResultadoProbabilidadeDto>
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

            repositorioMock.Setup(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId))
                .ReturnsAsync(itens);

            var query = new ObterDownloadDreResultadoProbabilidadeQuery(loteId, dreId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(itens.Count, resultado.Count());

            repositorioMock.Verify(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar uma lista vazia quando o repositório retornar uma lista vazia")]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Retornar_Vazio()
        {
            var loteId = 1L;
            var dreId = 1;

            repositorioMock.Setup(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId))
                .ReturnsAsync(new List<DownloadResultadoProbabilidadeDto>());

            var query = new ObterDownloadDreResultadoProbabilidadeQuery(loteId, dreId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);

            repositorioMock.Verify(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar null quando o repositório retornar null")]
        public async Task Deve_Retornar_Null_Quando_Repositorio_Retornar_Null()
        {
            var loteId = 1L;
            var dreId = 1;

            repositorioMock.Setup(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId))
                .ReturnsAsync((IEnumerable<DownloadResultadoProbabilidadeDto>)null);

            var query = new ObterDownloadDreResultadoProbabilidadeQuery(loteId, dreId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);

            repositorioMock.Verify(r => r.ObterDownloadDreResultadoProbabilidade(loteId, dreId), Times.Once);
        }
    }
}
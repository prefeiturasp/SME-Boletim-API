using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterAnosEscolaresPorLoteIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioLoteProva> repositorioLoteProva;
        private readonly ObterAnosEscolaresPorLoteIdQueryHandler obterAnosEscolaresPorLoteIdQueryHandler;
        public ObterAnosEscolaresPorLoteIdQueryHandlerTeste()
        {
            this.repositorioLoteProva = new Mock<IRepositorioLoteProva>();
            this.obterAnosEscolaresPorLoteIdQueryHandler = new ObterAnosEscolaresPorLoteIdQueryHandler(repositorioLoteProva.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Escolares_Por_LoteId()
        {
            var anosEscolaresEsperados = new List<AnoEscolarDto>
            {
                new AnoEscolarDto { Ano = 5, Modalidade = Dominio.Enumerados.Modalidade.Fundamental },
                new AnoEscolarDto { Ano = 9, Modalidade = Dominio.Enumerados.Modalidade.Fundamental }
            };

            repositorioLoteProva.Setup(r => r.ObterAnosEscolaresPorLoteId(It.IsAny<long>())).ReturnsAsync(anosEscolaresEsperados);
            var resultado = await obterAnosEscolaresPorLoteIdQueryHandler.Handle(new ObterAnosEscolaresPorLoteIdQuery(It.IsAny<long>()), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anosEscolaresEsperados.Count, resultado.Count());
            Assert.Contains(resultado, x => anosEscolaresEsperados.Any(a => a.Ano == x.Ano));
            repositorioLoteProva.Verify(r => r.ObterAnosEscolaresPorLoteId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Anos_Escolares_Por_LoteId()
        {
            repositorioLoteProva.Setup(r => r.ObterAnosEscolaresPorLoteId(It.IsAny<long>())).ReturnsAsync(new List<AnoEscolarDto>());
            var resultado = await obterAnosEscolaresPorLoteIdQueryHandler.Handle(new ObterAnosEscolaresPorLoteIdQuery(It.IsAny<long>()), CancellationToken.None);

            Assert.Empty(resultado);
            repositorioLoteProva.Verify(r => r.ObterAnosEscolaresPorLoteId(It.IsAny<long>()), Times.Once);
        }
    }
}

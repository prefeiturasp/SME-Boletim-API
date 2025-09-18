using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAnosEscolaresPorDreAnoAplicacaoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterAnosEscolaresPorDreAnoAplicacaoQueryHandler handler;

        public ObterAnosEscolaresPorDreAnoAplicacaoQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterAnosEscolaresPorDreAnoAplicacaoQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_Anos_Escolares_Correta()
        {
            var dreId = 1L;
            var anoAplicacao = 2023;
            var disciplinaId = 1;

            var anosEscolaresEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "5"},
                new OpcaoFiltroDto<int> { Valor = 9, Texto = "9"},
            };

            repositorio
                .Setup(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId))
                .ReturnsAsync(anosEscolaresEsperados);

            var query = new ObterAnosEscolaresPorDreAnoAplicacaoQuery(dreId, anoAplicacao, disciplinaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anosEscolaresEsperados.Count, resultado.Count());
            Assert.Equal(anosEscolaresEsperados, resultado);

            repositorio.Verify(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Tiver_Dados()
        {
            var dreId = 2L;
            var anoAplicacao = 2024;
            var disciplinaId = 1;

            repositorio
                .Setup(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var query = new ObterAnosEscolaresPorDreAnoAplicacaoQuery(dreId, anoAplicacao, disciplinaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorio.Verify(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Repositorio_Falhar()
        {
            var dreId = 3L;
            var anoAplicacao = 2025;
            var disciplinaId = 1;

            var query = new ObterAnosEscolaresPorDreAnoAplicacaoQuery(dreId, anoAplicacao, disciplinaId);

            repositorio
                .Setup(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId))
                .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", exception.Message);

            repositorio.Verify(r => r.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId), Times.Once);
        }
    }
}

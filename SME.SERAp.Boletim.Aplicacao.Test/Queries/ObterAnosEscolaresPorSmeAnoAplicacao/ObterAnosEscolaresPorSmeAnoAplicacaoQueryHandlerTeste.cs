using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosEscolaresPorSmeAnoAplicacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterAnosEscolaresPorSmeAnoAplicacao
{
    public class ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandler handler;

        public ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Escolares_Quando_Repositorio_Tiver_Dados()
        {
            var anoAplicacao = 2023;
            var disciplinaId = 1;
            var anosEscolaresEsperados = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 1, Texto = "1º Ano" },
                new OpcaoFiltroDto<int> { Valor = 2, Texto = "2º Ano" }
            };

            repositorio
                .Setup(r => r.ObterAnosEscolaresPorSmeAnoAplicacao(anoAplicacao, disciplinaId))
                .ReturnsAsync(anosEscolaresEsperados);

            var query = new ObterAnosEscolaresPorSmeAnoAplicacaoQuery(anoAplicacao, disciplinaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anosEscolaresEsperados.Count, resultado.Count());
            Assert.Equal(anosEscolaresEsperados, resultado);
            repositorio.Verify(r => r.ObterAnosEscolaresPorSmeAnoAplicacao(anoAplicacao, disciplinaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Tiver_Dados()
        {
            var anoAplicacao = 2023;
            var disciplinaId = 1;
            var anosEscolaresEsperados = new List<OpcaoFiltroDto<int>>();

            repositorio
                .Setup(r => r.ObterAnosEscolaresPorSmeAnoAplicacao(anoAplicacao, disciplinaId))
                .ReturnsAsync(anosEscolaresEsperados);

            var query = new ObterAnosEscolaresPorSmeAnoAplicacaoQuery(anoAplicacao, disciplinaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterAnosEscolaresPorSmeAnoAplicacao(anoAplicacao, disciplinaId), Times.Once);
        }
    }
}
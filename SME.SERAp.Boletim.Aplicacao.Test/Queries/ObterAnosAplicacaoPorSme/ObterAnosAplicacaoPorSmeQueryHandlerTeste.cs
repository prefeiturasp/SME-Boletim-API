using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosAplicacaoPorSme;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterAnosAplicacaoPorSme
{
    public class ObterAnosAplicacaoPorSmeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterAnosAplicacaoPorSmeQueryHandler handler;

        public ObterAnosAplicacaoPorSmeQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterAnosAplicacaoPorSmeQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_Anos_Correta()
        {
            var anosEsperados = new List<int> { 2021, 2022, 2023 };

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorSme())
                .ReturnsAsync(anosEsperados);

            var query = new ObterAnosAplicacaoPorSmeQuery();

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anosEsperados.Count, resultado.Count());
            Assert.Equal(anosEsperados, resultado);
            repositorio.Verify(r => r.ObterAnosAplicacaoPorSme(), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Tiver_Dados()
        {
            var anosEsperados = new List<int>();

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorSme())
                .ReturnsAsync(anosEsperados);

            var query = new ObterAnosAplicacaoPorSmeQuery();

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterAnosAplicacaoPorSme(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Repositorio_Falhar()
        {
            var query = new ObterAnosAplicacaoPorSmeQuery();

            repositorio
                .Setup(r => r.ObterAnosAplicacaoPorSme())
                .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", exception.Message);
            repositorio.Verify(r => r.ObterAnosAplicacaoPorSme(), Times.Once);
        }
    }
}
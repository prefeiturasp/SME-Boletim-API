using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAnoLoteProvaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterAnoLoteProvaQueryHandler handler;

        public ObterAnoLoteProvaQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterAnoLoteProvaQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ano_Quando_Repositorio_Retornar_Valor()
        {
            var loteId = 123;
            var anoEsperado = 2025;
            var query = new ObterAnoLoteProvaQuery(loteId);

            repositorio.Setup(r => r.ObterAnoPorLoteIdAsync(loteId))
                            .ReturnsAsync(anoEsperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(anoEsperado, resultado);
            repositorio.Verify(r => r.ObterAnoPorLoteIdAsync(loteId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Zero_Quando_Repositorio_Nao_Encontrar_Ano()
        {
            var loteId = 456;
            var query = new ObterAnoLoteProvaQuery(loteId);

            repositorio.Setup(r => r.ObterAnoPorLoteIdAsync(loteId))
                            .ReturnsAsync(0);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(0, resultado);
            repositorio.Verify(r => r.ObterAnoPorLoteIdAsync(loteId), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var loteId = 999;
            var query = new ObterAnoLoteProvaQuery(loteId);

            repositorio.Setup(r => r.ObterAnoPorLoteIdAsync(loteId))
                            .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", excecao.Message);
            repositorio.Verify(r => r.ObterAnoPorLoteIdAsync(loteId), Times.Once);
        }
    }
}

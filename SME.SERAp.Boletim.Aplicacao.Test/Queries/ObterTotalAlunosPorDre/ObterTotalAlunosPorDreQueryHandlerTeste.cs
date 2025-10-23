using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalAlunosPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTotalAlunosPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterTotalAlunosPorDreQueryHandler handler;

        public ObterTotalAlunosPorDreQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterTotalAlunosPorDreQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Valor_Correto_Quando_Existir()
        {
            repositorio
                .Setup(r => r.ObterTotalAlunosPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(150);

            var requisicao = new ObterTotalAlunosPorDreQuery(10, 2, 5);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.Equal(150, resultado);
            repositorio.Verify(r =>
                r.ObterTotalAlunosPorDreAsync(10, 2, 5), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Zero_Quando_Nao_Houver_Alunos()
        {
            repositorio
                .Setup(r => r.ObterTotalAlunosPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(0);

            var requisicao = new ObterTotalAlunosPorDreQuery(11, 3, 9);
            var resultado = await handler.Handle(requisicao, CancellationToken.None);

            Assert.Equal(0, resultado);
            repositorio.Verify(r =>
                r.ObterTotalAlunosPorDreAsync(11, 3, 9), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            repositorio
                .Setup(r => r.ObterTotalAlunosPorDreAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var requisicao = new ObterTotalAlunosPorDreQuery(12, 4, 9);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(requisicao, CancellationToken.None));

            repositorio.Verify(r =>
                r.ObterTotalAlunosPorDreAsync(12, 4, 9), Times.Once);
        }
    }
}

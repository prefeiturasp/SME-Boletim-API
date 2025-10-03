using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterComponentesCurricularesSmePorAnoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterComponentesCurricularesSmePorAnoQueryHandler handler;

        public ObterComponentesCurricularesSmePorAnoQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterComponentesCurricularesSmePorAnoQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Componentes_Curriculares_Correta()
        {
            var request = new ObterComponentesCurricularesSmePorAnoQuery(2025);
            var esperado = new List<OpcaoFiltroDto<int>> { new OpcaoFiltroDto<int> { Valor = 1, Texto = "Teste" } };

            repositorio.Setup(r => r.ObterComponentesCurricularesSmePorAno(request.AnoAplicacao))
                .ReturnsAsync(esperado);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(esperado, resultado);
            repositorio.Verify(r => r.ObterComponentesCurricularesSmePorAno(2025), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_RetornaDados()
        {
            var request = new ObterComponentesCurricularesSmePorAnoQuery(2024);

            repositorio.Setup(r => r.ObterComponentesCurricularesSmePorAno(request.AnoAplicacao))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var request = new ObterComponentesCurricularesSmePorAnoQuery(2023);

            repositorio.Setup(r => r.ObterComponentesCurricularesSmePorAno(request.AnoAplicacao))
                .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}

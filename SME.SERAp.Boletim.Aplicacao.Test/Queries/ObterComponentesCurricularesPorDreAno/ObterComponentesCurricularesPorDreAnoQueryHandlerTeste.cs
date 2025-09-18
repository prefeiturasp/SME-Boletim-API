using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterComponentesCurricularesPorDreAno;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterComponentesCurricularesPorDreAnoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterComponentesCurricularesPorDreAnoQueryHandler handler;

        public ObterComponentesCurricularesPorDreAnoQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterComponentesCurricularesPorDreAnoQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Componentes_Curriculares_Correta()
        {
            var request = new ObterComponentesCurricularesPorDreAnoQuery(1, 2025);
            var esperado = new List<OpcaoFiltroDto<int>> { new OpcaoFiltroDto<int> { Valor = 1, Texto = "Teste"} };

            repositorio.Setup(r => r.ObterComponentesCurricularesPorDreAno(request.DreId, request.AnoAplicacao))
                .ReturnsAsync(esperado);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(esperado, resultado);
            repositorio.Verify(r => r.ObterComponentesCurricularesPorDreAno(1, 2025), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_RetornaDados()
        {
            var request = new ObterComponentesCurricularesPorDreAnoQuery(2, 2024);

            repositorio.Setup(r => r.ObterComponentesCurricularesPorDreAno(request.DreId, request.AnoAplicacao))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var request = new ObterComponentesCurricularesPorDreAnoQuery(3, 2023);

            repositorio.Setup(r => r.ObterComponentesCurricularesPorDreAno(request.DreId, request.AnoAplicacao))
                .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}

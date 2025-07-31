using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;


namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterAbaEstudanteGraficoPorUeIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimProvaAluno;
        private readonly ObterAbaEstudanteGraficoPorUeIdQueryHandler queryHandler;

        public ObterAbaEstudanteGraficoPorUeIdQueryHandlerTeste()
        {
            repositorioBoletimProvaAluno = new Mock<IRepositorioBoletimProvaAluno>();
            queryHandler = new ObterAbaEstudanteGraficoPorUeIdQueryHandler(repositorioBoletimProvaAluno.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Grafico_Estudante_Por_UeId()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudanteDto
            {
                Ano = new List<int> { 5 },
                ComponentesCurriculares = new List<int> { 1 },
                NivelMinimo = 200,
                NivelMaximo = 300,
                NomeEstudante = "Maria"
            };

            var resultadoEsperado = new List<AbaEstudanteGraficoDto>
            {
               new AbaEstudanteGraficoDto { Turma = "A", Disciplina = "Matemática", Alunos = new List<AbaEstudanteGraficoAlunoDto>() }
            };

            repositorioBoletimProvaAluno
                .Setup(r => r.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros))
                .ReturnsAsync(resultadoEsperado);

            var query = new ObterAbaEstudanteGraficoPorUeIdQuery(loteId, ueId, filtros);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, resultado.Count());
            Assert.All(resultado, item => Assert.False(string.IsNullOrWhiteSpace(item.Disciplina)));

            repositorioBoletimProvaAluno.Verify(r => r.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Grafico_Se_Nao_Houver_Dados()
        {
            var loteId = 2L;
            var ueId = 999L;
            var filtros = new FiltroBoletimEstudanteDto
            {
                Ano = new List<int> { 9 },
                ComponentesCurriculares = new List<int> { 2 }
            };

            repositorioBoletimProvaAluno
                .Setup(r => r.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros))
                .ReturnsAsync(new List<AbaEstudanteGraficoDto>());

            var query = new ObterAbaEstudanteGraficoPorUeIdQuery(loteId, ueId, filtros);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorioBoletimProvaAluno.Verify(r => r.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros), Times.Once);
        }
    }
}

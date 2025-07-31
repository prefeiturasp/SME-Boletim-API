using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimProvaAluno;
        private readonly ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandler queryHandler;

        public ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandlerTeste()
        {
            repositorioBoletimProvaAluno = new Mock<IRepositorioBoletimProvaAluno>();
            queryHandler = new ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandler(repositorioBoletimProvaAluno.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Grafico_Estudante_Por_UeId()
        {


            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudantePaginadoDto
            {
                Ano = new List<int> { 5 },
                ComponentesCurriculares = new List<int> { 1 },
                NivelMinimo = 200,
                NivelMaximo = 300,
                NomeEstudante = "Maria",
                PageNumber = 1,
                PageSize = 1

            };

            var resultadoEsperado = new List<AbaEstudanteListaDto>
            {
                new AbaEstudanteListaDto { Turma = "A", Disciplina = "Matemática", AlunoNome = "Maria" }
            };

            repositorioBoletimProvaAluno
                .Setup(r => r.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros))
                .ReturnsAsync((resultadoEsperado, resultadoEsperado.Count));

            var query = new ObterAbaEstudanteBoletimEscolarPorUeIdQuery(loteId, ueId, filtros);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado.estudantes);
            Assert.Equal(resultadoEsperado.Count, resultado.estudantes.Count());
            Assert.All(resultado.estudantes, item => Assert.False(string.IsNullOrWhiteSpace(item.Disciplina)));

            repositorioBoletimProvaAluno.Verify(r => r.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Grafico_Se_Nao_Houver_Dados()
        {
            var loteId = 2L;
            var ueId = 999L;
            var filtros = new FiltroBoletimEstudantePaginadoDto
            {
                Ano = new List<int> { 9 },
                ComponentesCurriculares = new List<int> { 2 }
            };

            repositorioBoletimProvaAluno
                .Setup(r => r.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros))
                .ReturnsAsync((Enumerable.Empty<AbaEstudanteListaDto>(), 0));

            var query = new ObterAbaEstudanteBoletimEscolarPorUeIdQuery(loteId, ueId, filtros);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado.estudantes);
            Assert.Empty(resultado.estudantes);
            Assert.Equal(0, resultado.totalRegistros);

            repositorioBoletimProvaAluno.Verify(r => r.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros), Times.Once);
        }
    }
}
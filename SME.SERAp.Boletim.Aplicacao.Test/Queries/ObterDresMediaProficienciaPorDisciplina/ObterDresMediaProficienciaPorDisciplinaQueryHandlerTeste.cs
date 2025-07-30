using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDresMediaProficienciaPorDisciplinaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterDresMediaProficienciaPorDisciplinaQueryHandler queryHandler;
        public ObterDresMediaProficienciaPorDisciplinaQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDresMediaProficienciaPorDisciplinaQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retorna_Dres_Media_Proficiencia_Por_Disciplina()
        {
            var itens = new List<DreDisciplinaMediaProficienciaDto>
            {
                new DreDisciplinaMediaProficienciaDto { DisciplinaId = 4, Disciplina = "Matemática", MediaProficiencia = 170.56M },
                new DreDisciplinaMediaProficienciaDto { DisciplinaId = 5, Disciplina = "Português", MediaProficiencia = 183.5M }
            };

            var loteId = 1;
            var anoEscolar = 5;
            var dresIds = new List<long>();

            repositorio.Setup(x => x.ObterDresMediaProficienciaPorDisciplina(loteId, anoEscolar, dresIds))
                .ReturnsAsync(itens);

            var resultado = await queryHandler.Handle(new ObterDresMediaProficienciaPorDisciplinaQuery(loteId, anoEscolar, dresIds), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(itens.Count, resultado.Count());
            repositorio.Verify(r => r.ObterDresMediaProficienciaPorDisciplina(loteId, anoEscolar, dresIds), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retorna_Dres_Media_Proficiencia_Por_Disciplina()
        {
            var loteId = 1;
            var anoEscolar = 5;
            var dresIds = new List<long>();

            repositorio.Setup(x => x.ObterDresMediaProficienciaPorDisciplina(loteId, anoEscolar, dresIds))
                .ReturnsAsync(new List<DreDisciplinaMediaProficienciaDto>());

            var resultado = await queryHandler.Handle(new ObterDresMediaProficienciaPorDisciplinaQuery(loteId, anoEscolar, dresIds), CancellationToken.None);

            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterDresMediaProficienciaPorDisciplina(loteId, anoEscolar, dresIds), Times.Once);
        }
    }
}

using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterNiveisProficienciaPorDisciplinaIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterNiveisProficienciaPorDisciplinaIdQueryHandler handler;

        public ObterNiveisProficienciaPorDisciplinaIdQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterNiveisProficienciaPorDisciplinaIdQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Niveis_Proficiencia_Quando_Repositorio_Retornar_Dados()
        {
            var disciplinaId = 10;
            var anoEscolar = 2025;

            var niveisEsperados = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = 4, Descricao = "Básico" },
                new ObterNivelProficienciaDto { DisciplinaId = 4, Descricao = "Avançado" }
            };

            repositorio
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(niveisEsperados);

            var query = new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Básico", resultado.First().Descricao);

            repositorio.Verify(r =>
                r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Retornar_Vazio()
        {
            var disciplinaId = 5;
            var anoEscolar = 2024;

            repositorio
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());

            var query = new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            repositorio.Verify(r =>
                r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Lancar_Excecao()
        {
            var disciplinaId = 99;
            var anoEscolar = 2023;

            repositorio
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ThrowsAsync(new System.Exception("Erro no repositório"));

            var query = new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar);

            await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(query, CancellationToken.None));

            repositorio.Verify(r =>
                r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar), Times.Once);
        }
    }
}

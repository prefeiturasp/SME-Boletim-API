using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterMediaProficienciaPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterMediaProficienciaPorDreQueryHandler queryHandler;

        public ObterMediaProficienciaPorDreQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterMediaProficienciaPorDreQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Medias_Proficiencia()
        {
            var loteId = 1L;
            var dreId = 10;
            var anoEscolar = 2023;

            var itens = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto
                {
                    DisciplinaId = 4,
                    DisciplinaNome = "Matemática",
                    MediaProficiencia = 250.5m
                }
            };

            repositorio.Setup(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar))
                       .ReturnsAsync(itens);

            var query = new ObterMediaProficienciaPorDreQuery(loteId, dreId, anoEscolar);
            var result = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(itens.Count, result.Count());
            Assert.Equal("Matemática", result.First().DisciplinaNome);

            repositorio.Verify(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Medias_Proficiencia()
        {
            var loteId = 1L;
            var dreId = 20;
            var anoEscolar = 2024;

            repositorio.Setup(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar))
                       .ReturnsAsync(new List<MediaProficienciaDisciplinaDto>());

            var query = new ObterMediaProficienciaPorDreQuery(loteId, dreId, anoEscolar);
            var result = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
            repositorio.Verify(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Exception_Quando_Repositorio_Falhar()
        {
            var loteId = 99L;
            var dreId = 30;
            var anoEscolar = 2025;

            repositorio.Setup(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar))
                       .ThrowsAsync(new Exception("Erro no repositório"));

            var query = new ObterMediaProficienciaPorDreQuery(loteId, dreId, anoEscolar);

            await Assert.ThrowsAsync<Exception>(() => queryHandler.Handle(query, CancellationToken.None));
            repositorio.Verify(r => r.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar), Times.Once);
        }
    }
}

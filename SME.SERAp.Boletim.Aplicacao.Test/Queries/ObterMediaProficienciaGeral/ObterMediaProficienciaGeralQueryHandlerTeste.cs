using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterMediaProficienciaGeralQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterMediaProficienciaGeralQueryHandler queryHandler;

        public ObterMediaProficienciaGeralQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterMediaProficienciaGeralQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Media_Proficiencia_Geral()
        {
            var itens = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto { DisciplinaId = 4, DisciplinaNome = "Matemática", MediaProficiencia = 170.56M },
                new MediaProficienciaDisciplinaDto { DisciplinaId = 5, DisciplinaNome = "Português", MediaProficiencia = 183.5M }
            };

            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            var query = new ObterMediaProficienciaGeralQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterMediaProficienciaGeral(loteId, anoEscolar)).ReturnsAsync(itens);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);
            
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(itens.Count, resultado.Count());
            repositorio.Verify(r => r.ObterMediaProficienciaGeral(loteId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Media_Proficiencia_Geral()
        {
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            var query = new ObterMediaProficienciaGeralQuery(loteId, anoEscolar);
            repositorio.Setup(r => r.ObterMediaProficienciaGeral(loteId, anoEscolar)).ReturnsAsync(new List<MediaProficienciaDisciplinaDto>());
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterMediaProficienciaGeral(loteId, anoEscolar), Times.Once);
        }
    }
}

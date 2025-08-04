using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaDre
{
    public class ObterProficienciaDreQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolar;
        private readonly ObterProficienciaDreQueryHandler queryHandler;

        public ObterProficienciaDreQueryHandlerTest()
        {
            repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterProficienciaDreQueryHandler(repositorioBoletimEscolar.Object);
        }

        [Fact(DisplayName = "Deve retornar a lista de DREs com proficiência agrupada e contagem correta")]
        public async Task Deve_Retornar_Lista_Proficiencia_Agrupada_Corretamente()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;
            var dadosPlano = ObterDadosPlanoDeProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(dadosPlano);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina); // Espera 2 disciplinas no total
            Assert.Equal(2, resultado.Itens.Count()); // Espera 2 DREs únicas

            var primeiraDre = resultado.Itens.First();
            Assert.Equal("DRE BUTANTA", primeiraDre.DreNome);
            Assert.Equal(2, primeiraDre.Disciplinas.Count()); // Espera 2 disciplinas para a primeira DRE
            Assert.Equal("Língua portuguesa", primeiraDre.Disciplinas.First().Disciplina);

            var segundaDre = resultado.Itens.Last();
            Assert.Equal("DRE ITAQUERA", segundaDre.DreNome);
            Assert.Single(segundaDre.Disciplinas); // Espera 1 disciplina para a segunda DRE
            Assert.Equal("Matemática", segundaDre.Disciplinas.First().Disciplina);

            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar objeto com listas vazias se o repositório retornar uma lista vazia")]
        public async Task Deve_Retornar_Objeto_Com_Listas_Vazias_Se_Repositorio_Retornar_Vazio()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(new List<DreDisciplinaProficienciaDto>());

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar objeto com listas vazias se o repositório retornar null")]
        public async Task Deve_Retornar_Objeto_Com_Listas_Vazias_Se_Repositorio_Retornar_Null()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync((IEnumerable<DreDisciplinaProficienciaDto>)null);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve filtrar por uma única DreId corretamente")]
        public async Task Deve_Filtrar_Por_Uma_Unica_DreId_Corretamente()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;
            var dreId = new List<long> { 1 };
            var dadosPlano = ObterDadosPlanoDeProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(dadosPlano);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Single(resultado.Itens);
            Assert.Equal("DRE BUTANTA", resultado.Itens.First().DreNome);
            Assert.Equal(1, resultado.Itens.First().DreId);
        }

        [Fact(DisplayName = "Deve filtrar por múltiplas DreIds corretamente")]
        public async Task Deve_Filtrar_Por_Multiplas_DreIds_Corretamente()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;
            var dreIds = new List<long> { 1, 2 };
            var dadosPlano = ObterDadosPlanoDeProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(dadosPlano);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Equal("DRE BUTANTA", resultado.Itens.First().DreNome);
            Assert.Equal("DRE ITAQUERA", resultado.Itens.Last().DreNome);
        }

        [Fact(DisplayName = "Deve retornar lista vazia se nenhuma DreId do filtro existir")]
        public async Task Deve_Retornar_Lista_Vazia_Se_Nenhuma_DreId_Do_Filtro_Existir()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;
            var dreIds = new List<long> { 99, 100 };
            var dadosPlano = ObterDadosPlanoDeProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(dadosPlano);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
        }

        private List<DreDisciplinaProficienciaDto> ObterDadosPlanoDeProficiencia()
        {
            return new List<DreDisciplinaProficienciaDto>
            {
                new DreDisciplinaProficienciaDto
                {
                    DreId = 1,
                    DreNome = "DRE BUTANTA",
                    AnoEscolar = 5,
                    TotalUes = 29,
                    TotalAlunos = 206,
                    TotalRealizaramProva = 1,
                    PercentualParticipacao = 0.49m,
                    Disciplina = "Língua portuguesa",
                    MediaProficiencia = 193.31m,
                    NivelProficiencia = "Básico"
                },
                new DreDisciplinaProficienciaDto
                {
                    DreId = 1,
                    DreNome = "DRE BUTANTA",
                    AnoEscolar = 5,
                    TotalUes = 29,
                    TotalAlunos = 206,
                    TotalRealizaramProva = 1,
                    PercentualParticipacao = 0.49m,
                    Disciplina = "Matemática",
                    MediaProficiencia = 176.67m,
                    NivelProficiencia = "Básico"
                },
                new DreDisciplinaProficienciaDto
                {
                    DreId = 2,
                    DreNome = "DRE ITAQUERA",
                    AnoEscolar = 5,
                    TotalUes = 15,
                    TotalAlunos = 150,
                    TotalRealizaramProva = 10,
                    PercentualParticipacao = 0.50m,
                    Disciplina = "Matemática",
                    MediaProficiencia = 180.00m,
                    NivelProficiencia = "Básico"
                }
            };
        }
    }
}
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterNiveisProficiencia
{
    public class ObterNiveisProficienciaQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimProvaAluno;
        private readonly ObterNiveisProficienciaQueryHandler handler;

        public ObterNiveisProficienciaQueryHandlerTest()
        {
            repositorioBoletimProvaAluno = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterNiveisProficienciaQueryHandler(repositorioBoletimProvaAluno.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Niveis_Proficiencia()
        {
            // Arrange
            var niveisProficiencia = new List<NivelProficienciaDto>
            {
                new NivelProficienciaDto
                {
                    DreId = 1,
                    Disciplina = "Matemática",
                    DisciplinaId = 101,
                    AnoEscolar = 5,
                    MediaProficiencia = 275.6m,
                    NivelCodigo = 3,
                    NivelDescricao = "Adequado"
                },
                new NivelProficienciaDto
                {
                    DreId = 2,
                    Disciplina = "Português",
                    DisciplinaId = 102,
                    AnoEscolar = 5,
                    MediaProficiencia = 210.4m,
                    NivelCodigo = 2,
                    NivelDescricao = "Básico"
                }
            };

            repositorioBoletimProvaAluno
                .Setup(x => x.ObterNiveisProficienciaDisciplinas(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterNiveisProficienciaQuery(5, 999);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(niveisProficiencia.Count, resultado.Count());

            Assert.Contains(resultado, x => x.Disciplina == "Matemática" &&
                                            x.DisciplinaId == 101 &&
                                            x.MediaProficiencia == 275.6m &&
                                            x.NivelDescricao == "Adequado");

            Assert.Contains(resultado, x => x.Disciplina == "Português" &&
                                            x.DisciplinaId == 102 &&
                                            x.MediaProficiencia == 210.4m &&
                                            x.NivelDescricao == "Básico");
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Niveis_Proficiencia()
        {
            // Arrange
            repositorioBoletimProvaAluno
                .Setup(x => x.ObterNiveisProficienciaDisciplinas(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new List<NivelProficienciaDto>());

            var query = new ObterNiveisProficienciaQuery(5, 123);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
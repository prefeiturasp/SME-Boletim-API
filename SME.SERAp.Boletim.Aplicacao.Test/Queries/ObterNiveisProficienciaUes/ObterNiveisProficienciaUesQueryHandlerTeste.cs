using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterNiveisProficienciaUesQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimProvaAluno;
        private readonly ObterNiveisProficienciaUesQueryHandler obterNiveisProficienciaUesQueryHandler;
        public ObterNiveisProficienciaUesQueryHandlerTeste()
        {
            this.repositorioBoletimProvaAluno = new Mock<IRepositorioBoletimProvaAluno>();
            this.obterNiveisProficienciaUesQueryHandler = new ObterNiveisProficienciaUesQueryHandler(repositorioBoletimProvaAluno.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Niveis_Proficiencia_Ues()
        {
            var niveisProficiencia = new List<UeNivelProficienciaDto>
            {
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "111", Nome = "UE Teste 1", MediaProficiencia = 150.32M, DisciplinaId = 4, Disciplina = "Matemática", NivelCodigo = 2, NivelDescricao = "Básico" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "112", Nome = "UE Teste 2", MediaProficiencia = 250.21M, DisciplinaId = 4, Disciplina = "Matemática", NivelCodigo = 4, NivelDescricao = "Avançado" }
            };

            repositorioBoletimProvaAluno.Setup(x => x.ObterNiveisProficienciaUes(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(niveisProficiencia);


            var resultado = await obterNiveisProficienciaUesQueryHandler.Handle(new ObterNiveisProficienciaUesQuery(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(niveisProficiencia.Count, resultado.Count());
            Assert.Contains(resultado, x => x.Codigo == "111" && x.MediaProficiencia == 150.32M);
            Assert.Contains(resultado, x => x.Codigo == "112" && x.MediaProficiencia == 250.21M);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Niveis_Proficiencia_Ues()
        {
            repositorioBoletimProvaAluno.Setup(x => x.ObterNiveisProficienciaUes(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new List<UeNivelProficienciaDto>());

            var resultado = await obterNiveisProficienciaUesQueryHandler.Handle(new ObterNiveisProficienciaUesQuery(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>()), CancellationToken.None);

            Assert.Empty(resultado);
        }
    }
}

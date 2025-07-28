using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterDresPorNivelProficienciaDisciplinaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDresPorNivelProficienciaDisciplinaUseCase useCase;

        public ObterDresPorNivelProficienciaDisciplinaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDresPorNivelProficienciaDisciplinaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Usuario_For_Nulo()
        {
            // Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), default))
                .ReturnsAsync((TipoPerfil?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(5, 1L));
            Assert.Equal("Usuário sem permissão.", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Perfil_Usuario_Nao_For_Administrador()
        {
            // Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), default))
                .ReturnsAsync(TipoPerfil.Diretor);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(5, 1L));
            Assert.Equal("Usuário sem permissão.", ex.Message);
        }

        [Fact]
        public async Task Deve_Retornar_Objeto_Vazio_Quando_Nao_Houver_Dados()
        {
            // Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), default))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaQuery>(), default))
                .ReturnsAsync(new List<NivelProficienciaDto>());

            // Act
            var resultado = await useCase.Executar(5, 1L);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.AnoEscolar);
            Assert.Equal(1L, resultado.LoteId);
            Assert.Empty(resultado.Disciplinas);
        }

        [Fact]
        public async Task Deve_Retornar_Disciplinas_Agrupadas_Por_Nivel()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 1L;

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), default))
                .ReturnsAsync(TipoPerfil.Administrador);

            var dados = ObterMockDados();

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaQuery>(), default))
                .ReturnsAsync(dados);

            // Act
            var resultado = await useCase.Executar(anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);
            Assert.Equal(loteId, resultado.LoteId);
            Assert.NotEmpty(resultado.Disciplinas);
            Assert.Equal(2, resultado.Disciplinas.Count);

            var matematica = resultado.Disciplinas.FirstOrDefault(d => d.DisciplinaId == 4);
            Assert.NotNull(matematica);
            Assert.Equal("Matemática", matematica.DisciplinaNome);
            Assert.Equal(2, matematica.DresPorNiveisProficiencia.Count);

            var basico = matematica.DresPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 2);
            Assert.NotNull(basico);
            Assert.Equal("Básico", basico.Descricao);
            Assert.Equal(1, basico.QuantidadeDres);

            var avancado = matematica.DresPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 4);
            Assert.NotNull(avancado);
            Assert.Equal("Avançado", avancado.Descricao);
            Assert.Equal(2, avancado.QuantidadeDres);
        }

        private List<NivelProficienciaDto> ObterMockDados()
        {
            return new List<NivelProficienciaDto>
            {
                new NivelProficienciaDto
                {
                    DisciplinaId = 4,
                    Disciplina = "Matemática",
                    NivelCodigo = 2,
                    NivelDescricao = "Básico"
                },
                new NivelProficienciaDto
                {
                    DisciplinaId = 4,
                    Disciplina = "Matemática",
                    NivelCodigo = 4,
                    NivelDescricao = "Avançado"
                },
                new NivelProficienciaDto
                {
                    DisciplinaId = 4,
                    Disciplina = "Matemática",
                    NivelCodigo = 4,
                    NivelDescricao = "Avançado"
                },
                new NivelProficienciaDto
                {
                    DisciplinaId = 5,
                    Disciplina = "Língua Portuguesa",
                    NivelCodigo = 2,
                    NivelDescricao = "Básico"
                }
            };
        }
    }
}
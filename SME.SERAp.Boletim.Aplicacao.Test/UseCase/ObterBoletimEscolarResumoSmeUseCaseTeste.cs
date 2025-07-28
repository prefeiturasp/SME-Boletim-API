using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarResumoSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterBoletimEscolarResumoSmeUseCase useCase;
        public ObterBoletimEscolarResumoSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterBoletimEscolarResumoSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resumo_Boletim_Escolar_Sme()
        {
            var tipoUsuario = TipoPerfil.Administrador;
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            var totalDres = 12;
            var totalUes = 1400;
            var totalAlunos = 120000;
            var mediasProficiencia = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto { DisciplinaId = 4, DisciplinaNome = "Matemática", MediaProficiencia = 170.56M },
                new MediaProficienciaDisciplinaDto { DisciplinaId = 5, DisciplinaNome = "Português", MediaProficiencia = 183.5M }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoUsuario);

            mediator.Setup(m => m.Send(It.IsAny<ObterTotalDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalDres);
            mediator.Setup(m => m.Send(It.IsAny<ObterTotalUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalUes);
            mediator.Setup(m => m.Send(It.IsAny<ObterTotalAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalAlunos);
            mediator.Setup(m => m.Send(It.IsAny<ObterMediaProficienciaGeralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediasProficiencia);

            var resultado = await useCase.Executar(loteId, anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(totalDres, resultado.TotalDres);
            Assert.Equal(totalUes, resultado.TotalUes);
            Assert.Equal(totalAlunos, resultado.TotalAlunos);
            Assert.Equal(mediasProficiencia.Count, resultado.ProficienciaDisciplina.Count());
            Assert.Equal(mediasProficiencia.Where(x => x.DisciplinaId == 4).Count(), resultado.ProficienciaDisciplina.Where(x => x.DisciplinaId == 4).Count());
            Assert.Equal(mediasProficiencia.Where(x => x.DisciplinaId == 5).Count(), resultado.ProficienciaDisciplina.Where(x => x.DisciplinaId == 5).Count());

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalDresQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalUesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalAlunosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterMediaProficienciaGeralQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Valores_Resumo_Boletim_Escolar_Sme()
        {
            var tipoUsuario = TipoPerfil.Administrador;
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            var totalDres = 0;
            var totalUes = 0;
            var totalAlunos = 0;
            var mediasProficiencia = new List<MediaProficienciaDisciplinaDto>();

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoUsuario);

            mediator.Setup(m => m.Send(It.IsAny<ObterTotalDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalDres);
            mediator.Setup(m => m.Send(It.IsAny<ObterTotalUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalUes);
            mediator.Setup(m => m.Send(It.IsAny<ObterTotalAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalAlunos);
            mediator.Setup(m => m.Send(It.IsAny<ObterMediaProficienciaGeralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediasProficiencia);

            var resultado = await useCase.Executar(loteId, anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(totalDres, resultado.TotalDres);
            Assert.Equal(totalUes, resultado.TotalUes);
            Assert.Equal(totalAlunos, resultado.TotalAlunos);
            Assert.Empty(resultado.ProficienciaDisciplina);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalDresQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalUesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalAlunosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterMediaProficienciaGeralQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Excecao_Nao_Autorizado()
        {
            var tipoUsuario = TipoPerfil.Administrador_DRE;
            var loteId = It.IsAny<long>();
            var anoEscolar = It.IsAny<int>();

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoUsuario);
            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, anoEscolar));
            Assert.Equal("Usuário sem permissão.", ex.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalDresQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalUesQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterTotalAlunosQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterMediaProficienciaGeralQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

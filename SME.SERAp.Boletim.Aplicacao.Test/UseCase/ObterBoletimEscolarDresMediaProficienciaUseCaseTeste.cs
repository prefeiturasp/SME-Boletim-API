using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarDresMediaProficienciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterBoletimEscolarDresMediaProficienciaUseCase useCase;
        public ObterBoletimEscolarDresMediaProficienciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterBoletimEscolarDresMediaProficienciaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Media_Proficiencia()
        {
            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            var loteId = 1L;
            var anoEscolar = 2023;
            var dresIds = new List<long>();

            var itens = new List<DreDisciplinaMediaProficienciaDto>
            {
                new DreDisciplinaMediaProficienciaDto { DreId = 1, DreNome = "DRE 1", DisciplinaId = 4, Disciplina = "Matemática", MediaProficiencia = 146.5M },
                new DreDisciplinaMediaProficienciaDto { DreId = 1, DreNome = "DRE 1", DisciplinaId = 5, Disciplina = "Português", MediaProficiencia = 203.54M },
                new DreDisciplinaMediaProficienciaDto { DreId = 2, DreNome = "DRE 2", DisciplinaId = 4, Disciplina = "Matemática", MediaProficiencia = 172.5M },
                new DreDisciplinaMediaProficienciaDto { DreId = 2, DreNome = "DRE 2", DisciplinaId = 5, Disciplina = "Português", MediaProficiencia = 183.06M }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            mediator.Setup(m => m.Send(It.IsAny<ObterDresMediaProficienciaPorDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(itens);

            var result = await useCase.Executar(loteId, anoEscolar, dresIds);

            Assert.NotNull(result);
            Assert.Equal(itens.GroupBy(x=> x.DreId).Count(), result.Count());
            Assert.Contains(result, r => r.DreId == 1 && r.DreNome == "DRE 1");
            Assert.Contains(result, r => r.DreId == 2 && r.DreNome == "DRE 2");

            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresMediaProficienciaPorDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

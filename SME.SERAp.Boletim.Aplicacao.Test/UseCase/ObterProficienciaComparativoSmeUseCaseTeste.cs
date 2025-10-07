using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSP;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterProficienciaComparativoSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterProficienciaComparativoSmeUseCase useCase;

        public ObterProficienciaComparativoSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterProficienciaComparativoSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Tabela_Completa_Com_Psa_E_Psp()
        {
            var anoLetivo = 2025;
            var disciplinaId = 1;
            var anoEscolar = 3;

            var proficienciasPsa = new List<ResultadoProeficienciaSme>
            {
                new ResultadoProeficienciaSme { NomeAplicacao = "PSA1", Periodo = "Jan", MediaProficiencia = 250, RealizaramProva = 100, QuantidadeUes = 10, QuantidadeDres = 3 },
                new ResultadoProeficienciaSme { NomeAplicacao = "PSA2", Periodo = "Jun", MediaProficiencia = 270, RealizaramProva = 120, QuantidadeUes = 11, QuantidadeDres = 4 }
            };

            var proficienciasPsp = new List<ResultadoProeficienciaSme>
            {
                new ResultadoProeficienciaSme { NomeAplicacao = "PSP", Periodo = "Dez", MediaProficiencia = 200, RealizaramProva = 80, QuantidadeUes = 9, QuantidadeDres = 2 }
            };

            var niveis = new List<ObterNivelProficienciaDto> { new ObterNivelProficienciaDto { DisciplinaId = 1, Ano = 5 , ValorReferencia = 250 } };

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficienciasPsa);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSPQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficienciasPsp);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(niveis);
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Avançado");

            var resultado = await useCase.Executar(anoLetivo, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Aplicacao.Count());
            Assert.All(resultado.Aplicacao, x => Assert.NotNull(x.NivelProficiencia));
            Assert.True(resultado.Variacao != 0);
        }

        [Fact]
        public async Task Deve_Retornar_Zeros_Quando_Psp_Vazio()
        {
            var proficienciasPsa = new List<ResultadoProeficienciaSme>
            {
                new ResultadoProeficienciaSme { NomeAplicacao = "PSA", Periodo = "Abr", MediaProficiencia = 220, RealizaramProva = 50, QuantidadeUes = 5, QuantidadeDres = 2 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficienciasPsa);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSPQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ResultadoProeficienciaSme>());
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Básico");

            var resultado = await useCase.Executar(2025, 1, 3);

            Assert.Single(resultado.Aplicacao.Where(x => x.Descricao == null));
            Assert.Equal(2, resultado.Aplicacao.Count());
        }

        [Fact]
        public async Task Deve_Retornar_Somente_Psp_Quando_Psa_Vazio()
        {
            var proficienciasPsp = new List<ResultadoProeficienciaSme>
            {
                new ResultadoProeficienciaSme { NomeAplicacao = "PSP", Periodo = "Dez", MediaProficiencia = 190, RealizaramProva = 80, QuantidadeUes = 8, QuantidadeDres = 3 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ResultadoProeficienciaSme>());
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSPQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficienciasPsp);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Intermediário");

            var resultado = await useCase.Executar(2025, 2, 4);

            Assert.Single(resultado.Aplicacao);
            Assert.Equal("PSP", resultado.Aplicacao.First().Descricao);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Mediator_Falhar()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("Erro no Mediator"));

            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(2025, 1, 3));
        }
    }
}

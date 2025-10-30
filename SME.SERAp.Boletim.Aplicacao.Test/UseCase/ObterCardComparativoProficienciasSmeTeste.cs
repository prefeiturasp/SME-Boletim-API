using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterCardComparativoProficienciasSmeTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterCardComparativoProficienciasSme useCase;

        public ObterCardComparativoProficienciasSmeTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterCardComparativoProficienciasSme(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Cards_Quando_DreId_For_Nulo()
        {
            var dres = new List<DreDto>
            {
                new DreDto { DreId = 1, DreNomeAbreviado = "D1", DreNome = "Dre Norte" }
            };

            var resultadoProeficienciaPorDre = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = 1, DreAbreviacao = "D1", DreNome = "Dre Norte", MediaProficiencia = 200, NomeAplicacao = "Prova 1", Periodo = "1º" }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dres);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto> { new ObterNivelProficienciaDto() });
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoProeficienciaPorDre);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoProeficienciaPorDre);
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Básico");

            var resultado = await useCase.Executar(2024, 1, 5);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Dres);
            Assert.Equal(1, resultado.Total);
            Assert.Equal(1, resultado.Pagina);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Cards_Quando_DreId_For_Definido()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Avançado");

            var resultado = await useCase.Executar(2024, 1, 5, 99);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Dres);
        }

        [Fact]
        public async Task Executar_Deve_Paginar_Corretamente()
        {
            var dres = new List<DreDto>
            {
                new DreDto { DreId = 1, DreNomeAbreviado = "D1", DreNome = "A" },
                new DreDto { DreId = 2, DreNomeAbreviado = "D2", DreNome = "B" },
                new DreDto { DreId = 3, DreNomeAbreviado = "D3", DreNome = "C" }
            };

            var resultadoProeficienciaPorDre = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = 1, DreAbreviacao = "D1", DreNome = "A", MediaProficiencia = 100 },
                new ResultadoProeficienciaPorDre { DreId = 2, DreAbreviacao = "D2", DreNome = "B", MediaProficiencia = 200 },
                new ResultadoProeficienciaPorDre { DreId = 3, DreAbreviacao = "D3", DreNome = "C", MediaProficiencia = 300 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dres);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoProeficienciaPorDre);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoProeficienciaPorDre);
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Intermediário");

            var resultado = await useCase.Executar(2024, 1, 5, null, 1, 2);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.ItensPorPagina);
            Assert.Equal(1, resultado.Pagina);
        }

        [Fact]
        public async Task CriaCardComparativoDre_Deve_Mapear_Corretamente_Quando_Proficiencias_Existem()
        {
            var niveis = new List<ObterNivelProficienciaDto>();
            var proficiencias = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreAbreviacao = "D1", DreNome = "Dre Norte", MediaProficiencia = 200, NomeAplicacao = "Prova 1", Periodo = "1º" }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficiencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(proficiencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Avançado");

            var metodo = typeof(ObterCardComparativoProficienciasSme).GetMethod("CriaCardComparativoDre", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<CardComparativoProficienciaDreDto>)metodo.Invoke(useCase, new object[] { 1, 2024, 1, 5, niveis });
            var resultado = await task;

            Assert.NotNull(resultado);
            Assert.Equal("Dre Norte", resultado.DreNome);
            Assert.NotNull(resultado.AplicacaoPsp);
        }

        [Fact]
        public async Task RetornaTodasAsDresSeDreIdIsNull_Deve_Retornar_Lista_Com_DreId()
        {
            var dres = new List<DreDto>
            {
                new DreDto { DreId = 10 }
            };
            mediator.Setup(m => m.Send(It.IsAny<ObterDresComparativoSmeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dres);

            var metodo = typeof(ObterCardComparativoProficienciasSme).GetMethod("retornaTodasAsDresSeDreIdIsNull", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<List<int?>>)metodo.Invoke(useCase, new object[] { null, 2024, 1, 5 });
            var resultado = await task;

            Assert.Single(resultado);
            Assert.Equal(10, resultado.First());
        }

        [Fact]
        public void CalculaVariacao_Deve_Retornar_Zero_Quando_Listas_Vazias()
        {
            var metodo = typeof(ObterCardComparativoProficienciasSme).GetMethod("calculaVariacao", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var resultado = (decimal)metodo.Invoke(null, new object[] { new List<ResultadoProeficienciaPorDre>(), null });
            Assert.Equal(0, resultado);
        }
    }
}

using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaProvaSaberesPorDre
{
    public class ObterProficienciaProvaSaberesPorDreQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterProficienciaProvaSaberesPorDreQueryHandler handler;

        private const int DRE_ID = 1;
        private const int ANO_LETIVO = 2024;
        private const int DISCIPLINA_ID = 10;
        private const int ANO_ESCOLAR = 5;

        public ObterProficienciaProvaSaberesPorDreQueryHandlerTest()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterProficienciaProvaSaberesPorDreQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVazia_QuandoNaoExistiremDados()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarDadosCorretamente_QuandoExistiremDados()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 600, RealizaramProva = 100, Periodo = "2024.1" },
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 650, RealizaramProva = 120, Periodo = "2024.2" }
            };

            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal(600, resultado.First().MediaProficiencia);
            Assert.Equal(650, resultado.Last().MediaProficiencia);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);

            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            await handler.Handle(query, CancellationToken.None);

            repositorioMock.Verify(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarProficienciaComValoresEsperados()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 700, RealizaramProva = 150, Periodo = "2024.1" }
            };

            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            var prof = resultado.First();
            Assert.Equal(700, prof.MediaProficiencia);
            Assert.Equal(150, prof.RealizaramProva);
            Assert.Equal("2024.1", prof.Periodo);
        }

        [Fact]
        public async Task Handle_DeveRetornarMultiplosResultadosComPeriodosDiferentes()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 600, RealizaramProva = 100, Periodo = "2024.1" },
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 650, RealizaramProva = 120, Periodo = "2024.2" },
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 680, RealizaramProva = 130, Periodo = "2024.3" }
            };

            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            Assert.Contains(resultado, r => r.Periodo == "2024.1");
            Assert.Contains(resultado, r => r.Periodo == "2024.2");
            Assert.Contains(resultado, r => r.Periodo == "2024.3");
        }

        [Fact]
        public async Task Handle_DeveRetornarProficienciaComValoresZero()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DreNome = "Diretoria A", MediaProficiencia = 0, RealizaramProva = 0, Periodo = "2024.1" }
            };

            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            var prof = resultado.First();
            Assert.Equal(0, prof.MediaProficiencia);
            Assert.Equal(0, prof.RealizaramProva);
        }

        [Fact]
        public async Task Handle_DeveRetornarNulo()
        {
            var query = new ObterProficienciaProvaSaberesPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            repositorioMock.Setup(r => r.ObterProficienciaDreProvaSaberesAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync((List<ResultadoProeficienciaPorDre>)null);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
            
        }
    }
}
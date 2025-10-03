using FluentAssertions;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaProvaSPAPorDre
{
    public class ObterProficienciaProvaSPAPorDreQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly ObterProficienciaProvaSPAPorDreQueryHandler handler;

        private const int DRE_ID = 1;
        private const int ANO_LETIVO = 2024;
        private const int DISCIPLINA_ID = 10;
        private const int ANO_ESCOLAR = 5;

        public ObterProficienciaProvaSPAPorDreQueryHandlerTest()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterProficienciaProvaSPAPorDreQueryHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVazia_QuandoNaoExistiremDados()
        {
            
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            
            var resultado = await handler.Handle(query, CancellationToken.None);

            
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_DeveRetornarDadosCorretamente_QuandoExistiremDados()
        {
            
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DisciplinaId = DISCIPLINA_ID, MediaProficiencia = 600 },
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE A", DisciplinaId = DISCIPLINA_ID, MediaProficiencia = 650 }
            };

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            
            var resultado = await handler.Handle(query, CancellationToken.None);

            
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.First().MediaProficiencia.Should().Be(600);
            resultado.Last().MediaProficiencia.Should().Be(650);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            
            await handler.Handle(query, CancellationToken.None);
            repositorioMock.Verify(r => r.ObterProficienciaPorDreProvaSPAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR), Times.Once);
        }

       
        [Fact]
        public async Task Handle_DeveRetornarUmUnicoRegistro_QuandoRepositorioRetornarUmItem()
        {
             
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE Única", DisciplinaId = DISCIPLINA_ID, MediaProficiencia = 700 }
            };

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

             
            var resultado = await handler.Handle(query, CancellationToken.None);

             
            resultado.Should().ContainSingle();
            resultado.First().MediaProficiencia.Should().Be(700);
        }

        [Theory]
        [InlineData(1, 2023, 10, 5)]
        [InlineData(2, 2024, 20, 9)]
        [InlineData(3, 2025, 30, 1)]
        public async Task Handle_DeveChamarRepositorioComVariosParametros(int dreId, int anoLetivo, int disciplinaId, int anoEscolar)
        {
             
            var query = new ObterProficienciaProvaSPAPorDreQuery(dreId, anoLetivo, disciplinaId, anoEscolar);

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            await handler.Handle(query, CancellationToken.None);

             
            repositorioMock.Verify(r => r.ObterProficienciaPorDreProvaSPAsync(dreId, anoLetivo, disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRespeitarCancellationToken()
        {
             
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var resultado = await handler.Handle(query, cts.Token);

             
            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_DeveRetornarProficienciaZero_QuandoRepositorioRetornarValorZero()
        {
             
            var query = new ObterProficienciaProvaSPAPorDreQuery(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR);
            var esperado = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { DreId = DRE_ID, DreAbreviacao = "DRE Zero", DisciplinaId = DISCIPLINA_ID, MediaProficiencia = 0 }
            };

            repositorioMock.Setup(r => r.ObterProficienciaPorDreProvaSPAsync(DRE_ID, ANO_LETIVO, DISCIPLINA_ID, ANO_ESCOLAR))
                           .ReturnsAsync(esperado);

            var resultado = await handler.Handle(query, CancellationToken.None);
            resultado.Should().ContainSingle();
            resultado.First().MediaProficiencia.Should().Be(0);
        }
    }
}
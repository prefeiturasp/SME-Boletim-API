using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;


namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId
{
    public class ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimMock;
        private readonly ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandler handler;

        public ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandlerTeste()
        {
            repositorioBoletimMock = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandler(repositorioBoletimMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Boletins_Quando_Existir()
        {
            // Arrange
            var loteId = 1L;
            var ueId = 123L;
            var provaId = 10L;
            var filtros = new FiltroBoletimDto();
            var boletinsEsperados = new List<TurmaBoletimEscolarDto>
    {
        new TurmaBoletimEscolarDto
        {
            Turma = "A",
            AbaixoBasico = 5,
            AbaixoBasicoPorcentagem = 10,
            Basico = 8,
            BasicoPorcentagem = 16,
            Adequado = 12,
            AdequadoPorcentagem = 24,
            Avancado = 15,
            AvancadoPorcentagem = 30,
            Total = 40,
            MediaProficiencia = 250
        }
    };

            repositorioBoletimMock
                .Setup(r => r.ObterBoletinsEscolaresTurmasPorUeIdProvaId(loteId, ueId, provaId, filtros))
                .ReturnsAsync(boletinsEsperados);

            var query = new ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(loteId, ueId, provaId, filtros);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            var turmaBoletim = ((List<TurmaBoletimEscolarDto>)resultado)[0];
            Assert.Equal("A", turmaBoletim.Turma);
            Assert.Equal(5, turmaBoletim.AbaixoBasico);
            Assert.Equal(10, turmaBoletim.AbaixoBasicoPorcentagem);
            Assert.Equal(8, turmaBoletim.Basico);
            Assert.Equal(16, turmaBoletim.BasicoPorcentagem);
            Assert.Equal(12, turmaBoletim.Adequado);
            Assert.Equal(24, turmaBoletim.AdequadoPorcentagem);
            Assert.Equal(15, turmaBoletim.Avancado);
            Assert.Equal(30, turmaBoletim.AvancadoPorcentagem);
            Assert.Equal(40, turmaBoletim.Total);
            Assert.Equal(250, turmaBoletim.MediaProficiencia);
        }

        [Fact]
        public async Task Deve_Retornar_Nulo_Quando_Nao_Existir_Boletins()
        {
            var loteId = 1L;
            var ueId = 999L;
            var provaId = 20L;
            var filtros = new FiltroBoletimDto();

            repositorioBoletimMock
                .Setup(r => r.ObterBoletinsEscolaresTurmasPorUeIdProvaId(loteId, ueId, provaId, filtros))
                .ReturnsAsync((IEnumerable<TurmaBoletimEscolarDto>)null);

            var query = new ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(loteId, ueId, provaId, filtros);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.Null(resultado);
        }
    }
}
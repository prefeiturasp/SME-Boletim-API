using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId
{
    public class ObterOpcoesTurmaBoletimEscolarPorUeIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_OpcoesTurma_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosTurma, loteId, ueId);

            var opcoesEsperadas = new List<OpcaoFiltroDto<string>>
            {
                new OpcaoFiltroDto<string> { Texto = "Turma A", Valor = "A" },
                new OpcaoFiltroDto<string> { Texto = "Turma B", Valor = "B" }
            };

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesTurmaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(opcoesEsperadas);

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<string>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(opcoesEsperadas);

            var handler = new ObterOpcoesTurmaBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesTurmaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<OpcaoFiltroDto<string>>)resultado).Count);
            Assert.Equal("Turma A", ((List<OpcaoFiltroDto<string>>)resultado)[0].Texto);
            Assert.Equal("A", ((List<OpcaoFiltroDto<string>>)resultado)[0].Valor);
            Assert.Equal("Turma B", ((List<OpcaoFiltroDto<string>>)resultado)[1].Texto);
            Assert.Equal("B", ((List<OpcaoFiltroDto<string>>)resultado)[1].Valor);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_OpcoesTurma()
        {
            var loteId = 1L;
            var ueId = 999L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosTurma, loteId, ueId);

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesTurmaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(new List<OpcaoFiltroDto<string>>());

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<string>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(new List<OpcaoFiltroDto<string>>());

            var handler = new ObterOpcoesTurmaBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesTurmaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
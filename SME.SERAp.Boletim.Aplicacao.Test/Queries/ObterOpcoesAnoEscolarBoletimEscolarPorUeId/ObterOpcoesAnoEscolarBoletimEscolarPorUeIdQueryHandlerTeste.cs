using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId
{
    public class ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_OpcoesAnoEscolar_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosAnoEscolar, loteId, ueId);

            var opcoesEsperadas = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Texto = "1º Ano", Valor = 1 },
                new OpcaoFiltroDto<int> { Texto = "2º Ano", Valor = 2 }
            };

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesAnoEscolarBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(opcoesEsperadas);

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<int>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(opcoesEsperadas);

            var handler = new ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<OpcaoFiltroDto<int>>)resultado).Count);
            Assert.Equal("1º Ano", ((List<OpcaoFiltroDto<int>>)resultado)[0].Texto);
            Assert.Equal(1, ((List<OpcaoFiltroDto<int>>)resultado)[0].Valor);
            Assert.Equal("2º Ano", ((List<OpcaoFiltroDto<int>>)resultado)[1].Texto);
            Assert.Equal(2, ((List<OpcaoFiltroDto<int>>)resultado)[1].Valor);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_OpcoesAnoEscolar()
        {
            var loteId = 1L;
            var ueId = 999L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosAnoEscolar, loteId, ueId);

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesAnoEscolarBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<int>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var handler = new ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
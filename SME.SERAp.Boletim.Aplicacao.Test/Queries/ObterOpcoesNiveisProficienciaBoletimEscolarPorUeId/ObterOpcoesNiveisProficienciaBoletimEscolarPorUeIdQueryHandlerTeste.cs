using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId
{
    public class ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_OpcoesNiveisProficiencia_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosNivelProficiencia, loteId, ueId);

            var opcoesEsperadas = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Texto = "Adequado", Valor = 1 },
                new OpcaoFiltroDto<int> { Texto = "Avançado", Valor = 2 }
            };

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(opcoesEsperadas);

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<int>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(opcoesEsperadas);

            var handler = new ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<OpcaoFiltroDto<int>>)resultado).Count);
            Assert.Equal("Adequado", ((List<OpcaoFiltroDto<int>>)resultado)[0].Texto);
            Assert.Equal(1, ((List<OpcaoFiltroDto<int>>)resultado)[0].Valor);
            Assert.Equal("Avançado", ((List<OpcaoFiltroDto<int>>)resultado)[1].Texto);
            Assert.Equal(2, ((List<OpcaoFiltroDto<int>>)resultado)[1].Valor);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_OpcoesNiveisProficiencia()
        {
            var loteId = 1L;
            var ueId = 999L;
            var chaveCache = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosNivelProficiencia, loteId, ueId);

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chaveCache,
                    It.IsAny<Func<Task<IEnumerable<OpcaoFiltroDto<int>>>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(new List<OpcaoFiltroDto<int>>());

            var handler = new ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQueryHandler(
                repositorioBoletimProvaAlunoMock.Object,
                repositorioCacheMock.Object);

            var query = new ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
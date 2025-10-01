using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId
{
    public class ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorioBoletimProvaAlunoMock;
        private readonly Mock<IRepositorioCache> repositorioCacheMock;
        private readonly ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandler handler;

        public ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandlerTest()
        {
            repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioCacheMock = new Mock<IRepositorioCache>();
            handler = new ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandler(repositorioBoletimProvaAlunoMock.Object, repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarValoresDoCache_QuandoExistir()
        {
            long loteId = 1;
            long ueId = 10;
            var esperado = new BoletimEscolarValoresNivelProficienciaDto { ValorMinimo = 100, ValorMaximo = 500 };
            var chave = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosValorProficiencia, loteId, ueId);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chave,
                    It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                    It.IsAny<int>()))
                .ReturnsAsync(esperado);

            var query = new ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.ValorMinimo, resultado.ValorMinimo);
            Assert.Equal(esperado.ValorMaximo, resultado.ValorMaximo);
            repositorioCacheMock.Verify(r => r.ObterRedisAsync(
                chave,
                It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                It.IsAny<int>()), Times.Once);
            repositorioBoletimProvaAlunoMock.Verify(r => r.ObterValoresNivelProficienciaBoletimEscolarPorUeId(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarValoresDoRepositorio_QuandoNaoExistirNoCache()
        {
            long loteId = 2;
            long ueId = 20;
            var esperado = new BoletimEscolarValoresNivelProficienciaDto { ValorMinimo = 200, ValorMaximo = 800 };
            var chave = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosValorProficiencia, loteId, ueId);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chave,
                    It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                    It.IsAny<int>()))
                .Returns<string, Func<Task<BoletimEscolarValoresNivelProficienciaDto>>, int>((_, buscarDados, __) => buscarDados());

            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterValoresNivelProficienciaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync(esperado);

            var query = new ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.ValorMinimo, resultado.ValorMinimo);
            Assert.Equal(esperado.ValorMaximo, resultado.ValorMaximo);
            repositorioCacheMock.Verify(r => r.ObterRedisAsync(
                chave,
                It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                It.IsAny<int>()), Times.Once);
            repositorioBoletimProvaAlunoMock.Verify(r => r.ObterValoresNivelProficienciaBoletimEscolarPorUeId(loteId, ueId), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarNulo_QuandoRepositorioRetornarNulo()
        {
            long loteId = 3;
            long ueId = 30;
            var chave = string.Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosValorProficiencia, loteId, ueId);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync(
                    chave,
                    It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                    It.IsAny<int>()))
                .Returns<string, Func<Task<BoletimEscolarValoresNivelProficienciaDto>>, int>((_, buscarDados, __) => buscarDados());

            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterValoresNivelProficienciaBoletimEscolarPorUeId(loteId, ueId))
                .ReturnsAsync((BoletimEscolarValoresNivelProficienciaDto)null);

            var query = new ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
            repositorioCacheMock.Verify(r => r.ObterRedisAsync(
                chave,
                It.IsAny<Func<Task<BoletimEscolarValoresNivelProficienciaDto>>>(),
                It.IsAny<int>()), Times.Once);
            repositorioBoletimProvaAlunoMock.Verify(r => r.ObterValoresNivelProficienciaBoletimEscolarPorUeId(loteId, ueId), Times.Once);
        }
    }
}
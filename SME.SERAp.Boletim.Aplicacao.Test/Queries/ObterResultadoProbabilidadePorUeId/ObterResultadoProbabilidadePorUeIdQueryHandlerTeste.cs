using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterResultadoProbabilidadePorUeIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterResultadoProbabilidadePorUeIdQueryHandler handler;

        public ObterResultadoProbabilidadePorUeIdQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterResultadoProbabilidadePorUeIdQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Quando_Repositorio_Retornar_Valores()
        {
            var loteId = 1L;
            var ueId = 2L;
            var disciplinaId = 3L;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Turma = new List<string> { "Turma A", "Turma B" },
                Habilidade = "Leitura",
                Pagina = 1,
                TamanhoPagina = 10
            };
            var query = new ObterResultadoProbabilidadePorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            var resultadosEsperados = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto(),
                new ResultadoProbabilidadeDto()
            };

            var totalEsperado = resultadosEsperados.Count;

            repositorio.Setup(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ReturnsAsync((resultadosEsperados, totalEsperado));

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(resultadosEsperados, resultado.Item1);
            Assert.Equal(totalEsperado, resultado.Item2);
            repositorio.Verify(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Retornar_Resultados()
        {
            var loteId = 10L;
            var ueId = 20L;
            var disciplinaId = 30L;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();
            var query = new ObterResultadoProbabilidadePorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            repositorio.Setup(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ReturnsAsync((Enumerable.Empty<ResultadoProbabilidadeDto>(), 0));

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado.Item1);
            Assert.Equal(0, resultado.Item2);
            repositorio.Verify(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var loteId = 999L;
            var ueId = 888L;
            var disciplinaId = 777L;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();
            var query = new ObterResultadoProbabilidadePorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            repositorio.Setup(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", excecao.Message);
            repositorio.Verify(r => r.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }
    }
}

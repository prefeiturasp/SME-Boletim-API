using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadeListaPorUeId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterResultadoProbabilidadeListaPorUeIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimProvaAluno> repositorio;
        private readonly ObterResultadoProbabilidadeListaPorUeIdQueryHandler handler;

        public ObterResultadoProbabilidadeListaPorUeIdQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            handler = new ObterResultadoProbabilidadeListaPorUeIdQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resultados_Quando_Repositorio_Retornar_Valores()
        {
            var loteId = 1;
            var ueId = 2;
            var disciplinaId = 3;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Turma = new List<string> { "Turma A", "Turma B" },
                Habilidade = "Leitura",
                Pagina = 1,
                TamanhoPagina = 10
            };
            var query = new ObterResultadoProbabilidadeListaPorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            var resultadosEsperados = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto(),
                new ResultadoProbabilidadeDto()
            };
            var totalEsperado = resultadosEsperados.Count;

            repositorio.Setup(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ReturnsAsync((resultadosEsperados, totalEsperado));

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(resultadosEsperados, resultado.Item1);
            Assert.Equal(totalEsperado, resultado.Item2);
            repositorio.Verify(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Retornar_Resultados()
        {
            var loteId = 10;
            var ueId = 20;
            var disciplinaId = 30;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();
            var query = new ObterResultadoProbabilidadeListaPorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            repositorio.Setup(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ReturnsAsync((Enumerable.Empty<ResultadoProbabilidadeDto>(), 0));

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado.Item1);
            Assert.Equal(0, resultado.Item2);
            repositorio.Verify(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var loteId = 999;
            var ueId = 888;
            var disciplinaId = 777;
            var anoEscolar = 2025;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();
            var query = new ObterResultadoProbabilidadeListaPorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros);

            repositorio.Setup(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros))
                        .ThrowsAsync(new InvalidOperationException("Erro no repositório"));

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro no repositório", excecao.Message);
            repositorio.Verify(r => r.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros), Times.Once);
        }
    }
}

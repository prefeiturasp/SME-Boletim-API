using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadeListaPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterResultadoProbabilidadePorUeListaUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_ResultadoPaginado_Quando_Houver_Dados()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Pagina = 1,
                TamanhoPagina = 10
            };

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new() { UeId = ueId }
            };

            var resultados = new List<ResultadoProbabilidadeDto>
            {
                new()
                {
                    CodigoHabilidade = "EF05MA01",
                    HabilidadeDescricao = "Resolver problemas matemáticos",
                    TurmaDescricao = "5A",
                    AbaixoDoBasico = 10.2m,
                    Basico = 20.5m,
                    Adequado = 30.3m,
                    Avancado = 39.0m
                }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResultadoProbabilidadeListaPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((resultados, 1));

            var useCase = new ObterResultadoProbabilidadePorUeListaUseCase(mediatorMock.Object);
            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.PaginaAtual);
            Assert.Equal(10, resultado.TamanhoPagina);
            Assert.Equal(1, resultado.TotalRegistros);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Single(resultado.Resultados);
            Assert.Equal("EF05MA01", resultado.Resultados.First().CodigoHabilidade);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_ResultadoVazio_Quando_Nao_Houver_Dados()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Pagina = 2,
                TamanhoPagina = 10
            };

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new() { UeId = ueId }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResultadoProbabilidadeListaPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Enumerable.Empty<ResultadoProbabilidadeDto>(), 0));

            var useCase = new ObterResultadoProbabilidadePorUeListaUseCase(mediatorMock.Object);
            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.PaginaAtual);
            Assert.Equal(10, resultado.TamanhoPagina);
            Assert.Equal(0, resultado.TotalRegistros);
            Assert.Equal(0, resultado.TotalPaginas);
            Assert.Empty(resultado.Resultados);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Usuario_Sem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<AbrangenciaUeDto>());

            var useCase = new ObterResultadoProbabilidadePorUeListaUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                useCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros));

            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }
    }
}

using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Linq;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterResultadoProbabilidadePorUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Resultado_Agrupado_Quando_Existir_Dados()
        {
            var loteId = 1L;
            var ueId = 101L;
            var disciplinaId = 2L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Pagina = 1,
                TamanhoPagina = 10
            };

            var abrangencias = new List<AbrangenciaUeDto>
    {
        new AbrangenciaUeDto { UeId = ueId }
    };

            var resultados = new List<ResultadoProbabilidadeDto>
    {
        new ResultadoProbabilidadeDto
        {
            CodigoHabilidade = "EF05MA01",
            HabilidadeDescricao = "Resolver problemas com números naturais.",
            TurmaDescricao = "5A",
            AbaixoDoBasico = 10,
            Basico = 20,
            Adequado = 30,
            Avancado = 40
        },
        new ResultadoProbabilidadeDto
        {
            CodigoHabilidade = "EF05MA01",
            HabilidadeDescricao = "Resolver problemas com números naturais.",
            TurmaDescricao = "5B",
            AbaixoDoBasico = 15,
            Basico = 25,
            Adequado = 35,
            Avancado = 25
        }
    };

            var retorno = ((IEnumerable<ResultadoProbabilidadeDto>)resultados, 2);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResultadoProbabilidadePorUeIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(retorno);

            var useCase = new ObterResultadoProbabilidadePorUeUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.PaginaAtual);
            Assert.Equal(10, resultado.TamanhoPagina);
            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Single(resultado.Resultados);

            var agrupado = resultado.Resultados.First();
            Assert.Equal("EF05MA01", agrupado.CodigoHabilidade);
            Assert.Equal(2, agrupado.Turmas.Count());
        }


        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            var useCase = new ObterResultadoProbabilidadePorUeUseCase(new Mock<IMediator>().Object);
            var loteId = 1L;
            var ueId = 1L;
            var disciplinaId = 1L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<AbrangenciaUeDto>());

            var sut = new ObterResultadoProbabilidadePorUeUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() =>
                sut.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros));

            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Resultados()
        {
            var loteId = 1L;
            var ueId = 101L;
            var disciplinaId = 2L;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto();

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResultadoProbabilidadePorUeIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(((IEnumerable<ResultadoProbabilidadeDto>, int))(null, 0));

            var useCase = new ObterResultadoProbabilidadePorUeUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Resultados);
            Assert.Equal(0, resultado.TotalPaginas);
        }
    }
}

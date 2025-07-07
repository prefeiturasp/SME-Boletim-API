using System.Text;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadResultadoProbabilidade;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDownloadResultadoProbabilidadeUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_MemoryStream_Com_HTML_Correto()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 5L;
            var anoEscolar = 7;

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new() { UeId = ueId, UeNome = "Escola Teste" }
            };

            var resultados = new List<DownloadResultadoProbabilidadeDto>
            {
                new()
                {
                    CodigoHabilidade = "EF05MA01",
                    HabilidadeDescricao = "Resolver problemas de adição",
                    TurmaDescricao = "5A",
                    AbaixoDoBasico = 10.5m,
                    Basico = 20.25m,
                    Adequado = 30.75m,
                    Avancado = 38.5m
                }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDownloadResultadoProbabilidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultados);

            var useCase = new ObterDownloadResultadoProbabilidadeUseCase(mediatorMock.Object);

            var result = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar);

            Assert.NotNull(result);
            Assert.IsType<MemoryStream>(result);

            result.Position = 0;
            using var reader = new StreamReader(result, Encoding.UTF8);
            var content = await reader.ReadToEndAsync();

            Assert.Contains("<table border='1'>", content);
            Assert.Contains("<td>EF05MA01</td>", content);
            Assert.Contains("<td>Resolver problemas de adição</td>", content);
            Assert.Contains("<td>5A</td>", content);
            Assert.Contains("<td>10,5</td>", content);
            Assert.Contains("<td>20,25</td>", content);
            Assert.Contains("<td>30,75</td>", content);
            Assert.Contains("<td>38,5</td>", content);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Se_Sem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 5L;
            var anoEscolar = 7;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>());

            var useCase = new ObterDownloadResultadoProbabilidadeUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId, disciplinaId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }
    }
}

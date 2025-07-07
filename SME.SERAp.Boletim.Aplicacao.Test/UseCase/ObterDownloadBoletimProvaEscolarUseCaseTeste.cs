using System.Text;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterDownloadBoletimProvaEscolarUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_MemoryStream_Com_Dados_HTML()
        {
            var loteId = 1L;
            var ueId = 123L;

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new() { UeId = ueId, UeNome = "Escola Teste" }
            };

            var dados = new List<DownloadProvasBoletimEscolarDto>
            {
                new()
                {
                    CodigoUE = "123456",
                    NomeUE = "Escola Teste",
                    AnoEscola = 5,
                    Turma = "5A",
                    AlunoRA = 987654,
                    NomeAluno = "João da Silva",
                    Componente = "Matemática",
                    Proficiencia = 250.5m,
                    Nivel = "Adequado"
                }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDownloadProvasBoletimEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var useCase = new ObterDownloadBoletimProvaEscolarUseCase(mediatorMock.Object);

            var result = await useCase.Executar(loteId, ueId);

            Assert.NotNull(result);
            Assert.IsType<MemoryStream>(result);

            result.Position = 0;
            using var reader = new StreamReader(result, Encoding.UTF8);
            var content = await reader.ReadToEndAsync();

            Assert.Contains("<table border='1'>", content);
            Assert.Contains("<td class=\"numero\">123456</td>", content);
            Assert.Contains("<td>Escola Teste</td>", content);
            Assert.Contains("<td class=\"numero\">5</td>", content);
            Assert.Contains("<td>5A</td>", content);
            Assert.Contains("<td class=\"numero\">987654</td>", content);
            Assert.Contains("<td>João da Silva</td>", content);
            Assert.Contains("<td>Matemática</td>", content);
            Assert.Contains("<td class=\"numero\">250,5</td>", content);
            Assert.Contains("<td>Adequado</td>", content);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Se_Usuario_Sem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 123L;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>());

            var useCase = new ObterDownloadBoletimProvaEscolarUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }
    }
}

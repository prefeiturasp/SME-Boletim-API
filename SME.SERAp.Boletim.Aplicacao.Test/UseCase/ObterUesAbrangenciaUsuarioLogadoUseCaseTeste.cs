using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase 
{
    public class ObterUesAbrangenciaUsuarioLogadoUseCaseTest
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Abrangencias_Do_Mediator()
        {
            var abrangenciasEsperadas = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = 1 },
                new AbrangenciaUeDto { UeId = 2 }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasEsperadas);

            var useCase = new ObterUesAbrangenciaUsuarioLogadoUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((ICollection<AbrangenciaUeDto>)resultado).Count);
            Assert.Contains(resultado, a => a.UeId == 1);
            Assert.Contains(resultado, a => a.UeId == 2);
        }
    }
}
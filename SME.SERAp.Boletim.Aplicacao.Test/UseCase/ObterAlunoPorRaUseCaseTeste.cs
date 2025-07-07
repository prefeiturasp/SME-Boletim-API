using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterAlunoPorRaUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Aluno_Quando_Encontrado()
        {
            var alunoRa = 123456L;
            var alunoEsperado = new Aluno
            {
                RA = alunoRa,
                Nome = "Aluno Teste"
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.Is<ObterAlunoSerapPorRaQuery>(q => q.AlunoRA == alunoRa), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoEsperado);

            var useCase = new ObterAlunoPorRaUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(alunoRa);

            Assert.NotNull(resultado);
            Assert.Equal(alunoEsperado.RA, resultado.RA);
            Assert.Equal(alunoEsperado.Nome, resultado.Nome);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Aluno_Nao_Encontrado()
        {
            var alunoRa = 999999L;

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoSerapPorRaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Aluno)null);

            var useCase = new ObterAlunoPorRaUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(alunoRa));
            Assert.Contains(alunoRa.ToString(), ex.Message);
        }
    }
}

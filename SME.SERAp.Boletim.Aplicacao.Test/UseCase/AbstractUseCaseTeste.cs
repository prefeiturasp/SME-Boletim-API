using System;
using MediatR;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.UseCase;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class AbstractUseCaseTeste
    {
        private class FakeUseCase : AbstractUseCase
        {
            public FakeUseCase(IMediator mediator) : base(mediator) { }

            public IMediator ObterMediator() => mediator;
        }

        [Fact]
        public void Construtor_Deve_Atribuir_Mediator_Quando_Nao_Nulo()
        {
            var mediatorMock = new Mock<IMediator>();

            var useCase = new FakeUseCase(mediatorMock.Object);

            Assert.NotNull(useCase.ObterMediator());
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_Nulo()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() => new FakeUseCase(null!));
            Assert.Equal("mediator", excecao.ParamName);
        }
    }
}

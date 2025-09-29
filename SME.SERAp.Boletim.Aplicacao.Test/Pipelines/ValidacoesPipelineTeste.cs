using FluentValidation;
using FluentValidation.Results;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Pipelines;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Test.Pipelines
{
    public class TesteRequest : MediatR.IRequest<string>
    {
        public string Valor { get; set; } = string.Empty;
    }

    public class ValidacoesPipelineTeste
    {
        private readonly Mock<IValidator<TesteRequest>> validador;
        private readonly ValidacoesPipeline<TesteRequest, string> pipeline;

        public ValidacoesPipelineTeste()
        {
            validador = new Mock<IValidator<TesteRequest>>();
            pipeline = new ValidacoesPipeline<TesteRequest, string>(new[] { validador.Object });
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Validadores_Nulos_No_Construtor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ValidacoesPipeline<TesteRequest, string>(null!));
        }

        [Fact]
        public async Task Deve_Chamar_Next_Quando_Nao_Houver_Validadores()
        {
            var pipelineSemValidadores = new ValidacoesPipeline<TesteRequest, string>(Enumerable.Empty<IValidator<TesteRequest>>());
            var request = new TesteRequest { Valor = "teste" };

            var resultado = await pipelineSemValidadores.Handle(request, () => Task.FromResult("ok"), CancellationToken.None);

            Assert.Equal("ok", resultado);
        }

        [Fact]
        public async Task Deve_Chamar_Next_Quando_Validadores_Nao_Retornarem_Erros()
        {
            validador
                .Setup(v => v.Validate(It.IsAny<ValidationContext<TesteRequest>>()))
                .Returns(new ValidationResult());

            var request = new TesteRequest { Valor = "teste" };

            var resultado = await pipeline.Handle(request, () => Task.FromResult("ok"), CancellationToken.None);

            Assert.Equal("ok", resultado);
        }

        [Fact]
        public async Task Deve_Lancar_ValidacaoException_Quando_Validadores_Retornarem_Erros()
        {
            validador
                .Setup(v => v.Validate(It.IsAny<ValidationContext<TesteRequest>>()))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                new ValidationFailure("Valor", "Erro de validação")
                }));

            var request = new TesteRequest { Valor = "teste" };

            await Assert.ThrowsAsync<ValidacaoException>(() =>
                pipeline.Handle(request, () => Task.FromResult("ok"), CancellationToken.None));
        }
    }
}

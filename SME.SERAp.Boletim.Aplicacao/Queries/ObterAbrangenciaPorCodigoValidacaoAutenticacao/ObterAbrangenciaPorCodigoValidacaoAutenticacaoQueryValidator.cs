using FluentValidation;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao
{
    public class ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidator : AbstractValidator<ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery>
    {
        public ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidator()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código de validação.");
        }
    }
}

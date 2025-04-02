using FluentValidation;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt
{
    public class ObterTokenJwtQueryValidator : AbstractValidator<ObterTokenJwtQuery>
    {
        public ObterTokenJwtQueryValidator()
        {
            RuleFor(x => x.Abrangencias)
                .NotNull()
                .NotEmpty()
                .WithMessage("Abrangências é obrigatório");
        }
    }
}

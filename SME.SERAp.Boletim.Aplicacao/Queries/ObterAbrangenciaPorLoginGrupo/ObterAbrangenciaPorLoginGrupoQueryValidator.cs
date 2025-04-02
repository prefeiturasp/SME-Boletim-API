using FluentValidation;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo
{
    public class ObterAbrangenciaPorLoginGrupoQueryValidator : AbstractValidator<ObterAbrangenciaPorLoginGrupoQuery>
    {
        public ObterAbrangenciaPorLoginGrupoQueryValidator()
        {
            RuleFor(c => c.Login)
               .NotEmpty()
               .WithMessage("O login ou código Rf é obrigatório.");

            RuleFor(c => c.Perfil)
                .NotEmpty()
                .WithMessage("O id do Grupo é obrigatório.");
        }
    }
}

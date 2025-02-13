using FluentValidation;
using MediatR;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Pipelines
{
    public class ValidacoesPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validadores;

        public ValidacoesPipeline(IEnumerable<IValidator<TRequest>> validadores)
        {
            this.validadores = validadores ?? throw new System.ArgumentNullException(nameof(validadores));
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validadores.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var erros = validadores
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null);

                if (erros != null && erros.Any())
                {
                    throw new ValidacaoException(erros);
                }
            }

            return next();
        }
    }
}
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Exceptions
{
    public class ValidacaoException : Exception
    {
        public readonly IEnumerable<ValidationFailure> Erros;

        public ValidacaoException(IEnumerable<ValidationFailure> erros) : base(erros.FirstOrDefault().ErrorMessage)
        {
            Erros = erros;

            foreach (var erro in erros)
            {
                if (!Data.Contains(erro.PropertyName))
                    Data.Add(erro.PropertyName, erro.ErrorMessage);
            }
        }

        public IEnumerable<string> Mensagens() => Erros?.Select(c => c.ErrorMessage);
    }
}
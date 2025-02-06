using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Exceptions
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem, int statusCode = 409) : base(mensagem)
        {
            StatusCode = statusCode;
        }

        public NegocioException(string mensagem, HttpStatusCode statusCode) : base(mensagem)
        {
            StatusCode = (int)statusCode;
        }

        public int StatusCode { get; }
    }
}
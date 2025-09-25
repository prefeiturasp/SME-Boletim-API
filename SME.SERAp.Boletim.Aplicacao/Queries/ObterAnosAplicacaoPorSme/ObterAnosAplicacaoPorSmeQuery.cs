using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosAplicacaoPorSme
{
    public class ObterAnosAplicacaoPorSmeQuery : IRequest<IEnumerable<int>>
    {
        public ObterAnosAplicacaoPorSmeQuery()
        {
        }
    }
}
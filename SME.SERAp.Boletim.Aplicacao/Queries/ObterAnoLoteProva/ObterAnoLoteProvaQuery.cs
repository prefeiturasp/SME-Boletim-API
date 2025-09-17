using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva
{
    public class ObterAnoLoteProvaQuery : IRequest<int>
    {
        public ObterAnoLoteProvaQuery(long loteId)
        {
            LoteId = loteId;
        }
        public long LoteId { get; set; }
    }
}
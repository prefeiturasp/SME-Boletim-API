using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre
{
    public class ObterTotalAlunosPorDreQuery : IRequest<int>
    {
        public ObterTotalAlunosPorDreQuery(long loteId, long dreId, int anoEscolar)
        {
            LoteId = loteId;
            DreId = dreId;
            AnoEscolar = anoEscolar;
        }

        public long LoteId { get; }
        public long DreId { get; }
        public int AnoEscolar { get; }
    }
}
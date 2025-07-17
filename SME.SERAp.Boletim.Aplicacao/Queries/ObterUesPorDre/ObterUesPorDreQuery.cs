using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre
{
    public class ObterUesPorDreQuery : IRequest<IEnumerable<UePorDreDto>>
    {
        public long DreId { get; }
        public int AnoEscolar { get; }
        public long LoteId { get; }

        public ObterUesPorDreQuery(long dreId, int anoEscolar, long loteId)
        {
            DreId = dreId;
            AnoEscolar = anoEscolar;
            LoteId = loteId;
        }
    }
}
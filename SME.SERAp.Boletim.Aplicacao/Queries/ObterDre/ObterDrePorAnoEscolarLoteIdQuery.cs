using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDre
{
    public class ObterDrePorAnoEscolarLoteIdQuery : IRequest<IEnumerable<DreDto>>
    {
        public int AnoEscolar { get; }
        public long LoteId { get; }

        public ObterDrePorAnoEscolarLoteIdQuery(int anoEscolar, long loteId)
        {
            AnoEscolar = anoEscolar;
            LoteId = loteId;
        }
    }
}
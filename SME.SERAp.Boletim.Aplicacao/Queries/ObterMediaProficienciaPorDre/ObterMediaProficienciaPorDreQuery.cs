using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre
{
    public class ObterMediaProficienciaPorDreQuery : IRequest<IEnumerable<MediaProficienciaDisciplinaDto>>
    {
        public ObterMediaProficienciaPorDreQuery(long loteId, long dreId, int anoEscolar)
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
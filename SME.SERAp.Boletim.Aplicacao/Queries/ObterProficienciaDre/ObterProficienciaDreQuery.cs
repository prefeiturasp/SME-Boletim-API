using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre
{
    public class ObterProficienciaDreQuery : IRequest<ProficienciaDreCompletoDto>
    {
        public int AnoEscolar { get; set; }
        public long LoteId { get; set; }
        public IEnumerable<long> DreIds { get; set; }

        public ObterProficienciaDreQuery(int anoEscolar, long loteId, IEnumerable<long> dreIds = null)
        {
            AnoEscolar = anoEscolar;
            LoteId = loteId;
            DreIds = dreIds;
        }
    }
}
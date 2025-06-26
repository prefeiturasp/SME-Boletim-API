using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId
{
    public class ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery : IRequest<IEnumerable<TurmaBoletimEscolarDto>>
    {
        public ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(long loteId, long ueId, long provaId, FiltroBoletimDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            ProvaId = provaId;
            Filtros = filtros;
        }

        public long LoteId { get; set; }    

        public long UeId { get; set; }

        public long ProvaId { get; set; }

        public  FiltroBoletimDto Filtros { get; set; }
    }
}

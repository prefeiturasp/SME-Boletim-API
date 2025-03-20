using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId
{
    public class ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery : IRequest<IEnumerable<TurmaBoletimEscolarDto>>
    {
        public ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(long ueId, long provaId, FiltroBoletimDto filtros)
        {
            UeId = ueId;
            ProvaId = provaId;
            Filtros = filtros;
        }

        public long UeId { get; set; }

        public long ProvaId { get; set; }

        public  FiltroBoletimDto Filtros { get; set; }
    }
}

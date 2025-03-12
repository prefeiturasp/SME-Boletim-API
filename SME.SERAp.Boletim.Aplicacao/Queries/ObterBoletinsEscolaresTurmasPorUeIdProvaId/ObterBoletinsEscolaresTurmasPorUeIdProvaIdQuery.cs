using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId
{
    public class ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery : IRequest<IEnumerable<TurmaBoletimEscolarDto>>
    {
        public ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(long ueId, long provaId)
        {
            UeId = ueId;
            ProvaId = provaId;
        }

        public long UeId { get; set; }

        public long ProvaId { get; set; }
    }
}

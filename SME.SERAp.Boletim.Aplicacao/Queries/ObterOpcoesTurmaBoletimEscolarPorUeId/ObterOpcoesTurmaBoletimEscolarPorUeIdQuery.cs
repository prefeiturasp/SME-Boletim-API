using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId
{
    public class ObterOpcoesTurmaBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<string>>>
    {
        public ObterOpcoesTurmaBoletimEscolarPorUeIdQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get;set; }
    }
}

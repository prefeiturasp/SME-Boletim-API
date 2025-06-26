using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId
{
    public class ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
    }
}

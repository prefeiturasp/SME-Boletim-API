using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId
{
    public class ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get;set; }
    }
}

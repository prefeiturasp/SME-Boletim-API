using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId
{
    public class ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get;set; }
    }
}

using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId
{
    public class ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}

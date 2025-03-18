using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId
{
    public class ObterOpcoesTurmaBoletimEscolarPorUeIdQuery
        : IRequest<IEnumerable<OpcaoFiltroDto<string>>>
    {
        public ObterOpcoesTurmaBoletimEscolarPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get;set; }
    }
}

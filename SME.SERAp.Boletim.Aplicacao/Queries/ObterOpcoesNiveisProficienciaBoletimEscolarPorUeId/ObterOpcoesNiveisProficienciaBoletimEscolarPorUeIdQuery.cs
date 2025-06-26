using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId
{
    public class ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
    }
}

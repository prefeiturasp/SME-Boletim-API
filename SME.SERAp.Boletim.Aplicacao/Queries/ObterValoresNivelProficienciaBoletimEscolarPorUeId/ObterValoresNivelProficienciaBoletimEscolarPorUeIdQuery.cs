using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId
{
    public class ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery
        : IRequest<BoletimEscolarValoresNivelProficienciaDto>
    {
        public ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
    }
}

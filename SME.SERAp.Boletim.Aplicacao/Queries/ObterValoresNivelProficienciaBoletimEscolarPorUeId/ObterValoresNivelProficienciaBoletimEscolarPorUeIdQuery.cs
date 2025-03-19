using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId
{
    public class ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery
        : IRequest<BoletimEscolarValoresNivelProficienciaDto>
    {
        public ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}

using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId
{
    public class ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery
        : IRequest<IEnumerable<NivelProficienciaBoletimEscolarDto>>
    {
        public ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(long ueId, long provaId)
        {
            UeId = ueId;
            ProvaId = provaId;
        }

        public long UeId { get; set; }

        public long ProvaId { get; set; }
    }
}

using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId
{
    public class ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery
        : IRequest<IEnumerable<NivelProficienciaBoletimEscolarDto>>
    {
        public ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(long ueId, long provaId, FiltroBoletimDto filtros)
        {
            UeId = ueId;
            ProvaId = provaId;
            Filtros = filtros;
        }

        public long UeId { get; set; }

        public long ProvaId { get; set; }

        public FiltroBoletimDto Filtros { get; set; }
    }
}

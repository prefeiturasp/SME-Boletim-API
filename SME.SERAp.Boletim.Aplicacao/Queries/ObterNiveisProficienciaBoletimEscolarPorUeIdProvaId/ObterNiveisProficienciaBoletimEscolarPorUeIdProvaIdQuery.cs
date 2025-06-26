using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId
{
    public class ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery
        : IRequest<IEnumerable<NivelProficienciaBoletimEscolarDto>>
    {
        public ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(long loteId, long ueId, long provaId, FiltroBoletimDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            ProvaId = provaId;
            Filtros = filtros;
        }

        public long LoteId { get; set; }

        public long UeId { get; set; }

        public long ProvaId { get; set; }

        public FiltroBoletimDto Filtros { get; set; }
    }
}

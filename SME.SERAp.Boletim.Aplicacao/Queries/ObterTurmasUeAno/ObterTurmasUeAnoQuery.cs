using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTurmasUeAnoQuery : IRequest<IEnumerable<TurmaAnoDto>>
    {
        public long LoteId { get; set; }
        public long UeId { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }

        public ObterTurmasUeAnoQuery(long loteId, long ueId, int disciplinaId, int anoEscolar)
        {
            LoteId = loteId;
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }
}

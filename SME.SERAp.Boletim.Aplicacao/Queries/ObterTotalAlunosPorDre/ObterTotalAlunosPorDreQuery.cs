using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalAlunosPorDre
{
    public class ObterTotalAlunosPorDreQuery : IRequest<int>
    {
        public ObterTotalAlunosPorDreQuery(long loteId, long dreId, int anoEscolar)
        {
            LoteId = loteId;
            DreId = dreId;
            AnoEscolar = anoEscolar;
        }

        public long LoteId { get; }
        public long DreId { get; }
        public int AnoEscolar { get; }
    }
}
using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalAlunosQuery : IRequest<int>
    {
        public ObterTotalAlunosQuery(long loteId, int anoEscolar)
        {
            LoteId = loteId;
            AnoEscolar = anoEscolar;
        }
        public long LoteId { get; }
        public int AnoEscolar { get; }
    }
}

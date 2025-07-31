using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalUesQuery : IRequest<int>
    {
        public long LoteId { get; set; }

        public int AnoEscolar { get; set; }
        public ObterTotalUesQuery(long loteId, int anoEscolar)
        {
            LoteId = loteId;
            AnoEscolar = anoEscolar;
        }
    }
}

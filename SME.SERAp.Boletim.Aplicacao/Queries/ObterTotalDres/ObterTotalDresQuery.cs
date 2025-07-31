using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalDresQuery : IRequest<int>
    {
        public long LoteId { get; set; }
        public int AnoEscolar { get; set; }
        public ObterTotalDresQuery(long loteId, int anoEscolar)
        {
            LoteId = loteId;
            AnoEscolar = anoEscolar;
        }
    }
}

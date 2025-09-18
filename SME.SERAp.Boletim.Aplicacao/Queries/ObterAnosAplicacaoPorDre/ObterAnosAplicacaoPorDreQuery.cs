using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosAplicacaoPorDreQuery : IRequest<IEnumerable<int>>
    {
        public long DreId { get; set; }
        public ObterAnosAplicacaoPorDreQuery(long dreId)
        {
            DreId = dreId;
        }
    }
}

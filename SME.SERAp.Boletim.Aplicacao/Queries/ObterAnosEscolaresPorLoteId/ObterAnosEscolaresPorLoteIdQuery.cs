using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosEscolaresPorLoteIdQuery : IRequest<IEnumerable<AnoEscolarDto>>
    {
        public ObterAnosEscolaresPorLoteIdQuery(long loteId)
        {
            LoteId = loteId;
        }

        public long LoteId { get; set; }
    }
}

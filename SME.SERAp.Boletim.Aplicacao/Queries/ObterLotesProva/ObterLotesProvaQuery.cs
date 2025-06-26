using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProva
{
    public class ObterLotesProvaQuery : IRequest<IEnumerable<LoteProvaDto>>
    {
    }
}

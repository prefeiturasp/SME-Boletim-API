using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProva
{
    public class ObterLotesProvaQueryHandler
        : IRequestHandler<ObterLotesProvaQuery, IEnumerable<LoteProvaDto>>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public ObterLotesProvaQueryHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<IEnumerable<LoteProvaDto>> Handle(ObterLotesProvaQuery request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.ObterLotesProva();
        }
    }
}

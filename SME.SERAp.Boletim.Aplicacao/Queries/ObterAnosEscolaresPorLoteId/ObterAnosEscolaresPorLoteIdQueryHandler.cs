using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosEscolaresPorLoteIdQueryHandler : IRequestHandler<ObterAnosEscolaresPorLoteIdQuery, IEnumerable<AnoEscolarDto>>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public ObterAnosEscolaresPorLoteIdQueryHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<IEnumerable<AnoEscolarDto>> Handle(ObterAnosEscolaresPorLoteIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.ObterAnosEscolaresPorLoteId(request.LoteId);
        }
    }
}

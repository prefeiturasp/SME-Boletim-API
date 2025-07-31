using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalDresQueryHandler : IRequestHandler<ObterTotalDresQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterTotalDresQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }
        public Task<int> Handle(ObterTotalDresQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalDres(request.LoteId, request.AnoEscolar);
        }
    }
}

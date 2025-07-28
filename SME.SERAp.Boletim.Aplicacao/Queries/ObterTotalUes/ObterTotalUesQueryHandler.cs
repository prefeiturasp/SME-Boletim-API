using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalUesQueryHandler : IRequestHandler<ObterTotalUesQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterTotalUesQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }
        public Task<int> Handle(ObterTotalUesQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalUes(request.LoteId, request.AnoEscolar);
        }
    }
}

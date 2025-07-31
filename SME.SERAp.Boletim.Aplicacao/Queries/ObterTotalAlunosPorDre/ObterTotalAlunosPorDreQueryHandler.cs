using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalAlunosPorDre
{
    public class ObterTotalAlunosPorDreQueryHandler : IRequestHandler<ObterTotalAlunosPorDreQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterTotalAlunosPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int> Handle(ObterTotalAlunosPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalAlunosPorDreAsync(request.LoteId, request.DreId, request.AnoEscolar);
        }
    }
}
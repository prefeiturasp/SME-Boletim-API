using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTotalAlunosQueryHandler : IRequestHandler<ObterTotalAlunosQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterTotalAlunosQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }
        public Task<int> Handle(ObterTotalAlunosQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalAlunos(request.LoteId, request.AnoEscolar);
        }
    }
}

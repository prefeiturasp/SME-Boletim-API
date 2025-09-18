using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosAplicacaoPorDreQueryHandler : IRequestHandler<ObterAnosAplicacaoPorDreQuery, IEnumerable<int>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;
        public ObterAnosAplicacaoPorDreQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<int>> Handle(ObterAnosAplicacaoPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnosAplicacaoPorDre(request.DreId);
        }
    }
}

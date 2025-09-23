using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesComparacaoPorDreQueryHandler : IRequestHandler<ObterUesComparacaoPorDreQuery, IEnumerable<UePorDreDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;
        public ObterUesComparacaoPorDreQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<UePorDreDto>> Handle(ObterUesComparacaoPorDreQuery request, CancellationToken cancellationToken)
        {
           return repositorio.ObterUesComparacaoPorDre(request.DreId, request.AnoAplicacao, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadeListaPorUeId
{
    public class ObterResultadoProbabilidadeListaPorUeIdQueryHandler : IRequestHandler<ObterResultadoProbabilidadeListaPorUeIdQuery, (IEnumerable<ResultadoProbabilidadeDto>, int)>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterResultadoProbabilidadeListaPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> Handle(ObterResultadoProbabilidadeListaPorUeIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterResultadoProbabilidadeListaPorUeAsync(request.UeId, request.DisciplinaId, request.AnoEscolar, request.Filtros);
        }
    }
}

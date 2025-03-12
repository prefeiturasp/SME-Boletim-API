using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId
{
    public class ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandler
        : IRequestHandler<ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery, IEnumerable<TurmaBoletimEscolarDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterBoletinsEscolaresTurmasPorUeIdProvaIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<TurmaBoletimEscolarDto>> Handle(ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterBoletinsEscolaresTurmasPorUeIdProvaId(request.UeId, request.ProvaId);
        }
    }
}

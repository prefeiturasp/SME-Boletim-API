using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterTurmasUeAnoQueryHandler : IRequestHandler<ObterTurmasUeAnoQuery, IEnumerable<TurmaAnoDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;
        public ObterTurmasUeAnoQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<TurmaAnoDto>> Handle(ObterTurmasUeAnoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTurmasUeAno(request.LoteId, request.UeId, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

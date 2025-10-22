using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSP
{
    public class ObterProficienciaSmeProvaSPQueryHandler : IRequestHandler<ObterProficienciaSmeProvaSPQuery, IEnumerable<ResultadoProeficienciaSme>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimProvaAluno;
        public ObterProficienciaSmeProvaSPQueryHandler(IRepositorioBoletimEscolar repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<ResultadoProeficienciaSme>> Handle(ObterProficienciaSmeProvaSPQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterProficienciaSmeProvaSPAsync(request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

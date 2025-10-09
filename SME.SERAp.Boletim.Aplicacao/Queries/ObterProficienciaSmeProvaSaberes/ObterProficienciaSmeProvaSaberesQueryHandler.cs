using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSaberes
{
    public class ObterProficienciaSmeProvaSaberesQueryHandler : IRequestHandler<ObterProficienciaSmeProvaSaberesQuery, IEnumerable<ResultadoProeficienciaSme>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimProvaAluno;
        public ObterProficienciaSmeProvaSaberesQueryHandler(IRepositorioBoletimEscolar repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<ResultadoProeficienciaSme>> Handle(ObterProficienciaSmeProvaSaberesQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterProficienciaSmeProvaSaberesAsync(request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

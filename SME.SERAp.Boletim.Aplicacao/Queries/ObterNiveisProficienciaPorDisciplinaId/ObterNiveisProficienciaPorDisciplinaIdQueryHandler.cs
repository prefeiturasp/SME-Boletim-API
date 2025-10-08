using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId
{
    public class ObterNiveisProficienciaPorDisciplinaIdQueryHandler : IRequestHandler<ObterNiveisProficienciaPorDisciplinaIdQuery, IEnumerable<ObterNivelProficienciaDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterNiveisProficienciaPorDisciplinaIdQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<IEnumerable<ObterNivelProficienciaDto>> Handle(ObterNiveisProficienciaPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
           return repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(request.DisciplinaId, request.AnoEscolar);
        }
    }
}

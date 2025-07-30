using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDresMediaProficienciaPorDisciplinaQueryHandler : IRequestHandler<ObterDresMediaProficienciaPorDisciplinaQuery, IEnumerable<DreDisciplinaMediaProficienciaDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterDresMediaProficienciaPorDisciplinaQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<IEnumerable<DreDisciplinaMediaProficienciaDto>> Handle(ObterDresMediaProficienciaPorDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.ObterDresMediaProficienciaPorDisciplina(request.LoteId, request.AnoEscolar, request.DresIds);
        }
    }
}

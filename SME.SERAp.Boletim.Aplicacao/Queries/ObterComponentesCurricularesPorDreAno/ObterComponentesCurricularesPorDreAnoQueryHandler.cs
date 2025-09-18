using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterComponentesCurricularesPorDreAno
{
    public class ObterComponentesCurricularesPorDreAnoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorDreAnoQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;

        public ObterComponentesCurricularesPorDreAnoQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterComponentesCurricularesPorDreAnoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterComponentesCurricularesPorDreAno(request.DreId, request.AnoAplicacao);
        }
    }
}

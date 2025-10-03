using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterComponentesCurricularesSmePorAnoQueryHandler : IRequestHandler<ObterComponentesCurricularesSmePorAnoQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;
        public ObterComponentesCurricularesSmePorAnoQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterComponentesCurricularesSmePorAnoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterComponentesCurricularesSmePorAno(request.AnoAplicacao);
        }
    }
}

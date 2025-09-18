using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosEscolaresPorDreAnoAplicacaoQueryHandler : IRequestHandler<ObterAnosEscolaresPorDreAnoAplicacaoQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;
        public ObterAnosEscolaresPorDreAnoAplicacaoQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterAnosEscolaresPorDreAnoAplicacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnosEscolaresPorDreAnoAplicacao(request.DreId, request.AnoAplicacao, request.DisciplinaId);
        }
    }
}

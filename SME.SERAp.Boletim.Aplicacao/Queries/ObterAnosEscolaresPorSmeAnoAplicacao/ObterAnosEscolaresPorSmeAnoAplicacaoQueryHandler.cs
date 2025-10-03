using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosEscolaresPorSmeAnoAplicacao
{
    public class ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandler : IRequestHandler<ObterAnosEscolaresPorSmeAnoAplicacaoQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;

        public ObterAnosEscolaresPorSmeAnoAplicacaoQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterAnosEscolaresPorSmeAnoAplicacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnosEscolaresPorSmeAnoAplicacao(request.AnoAplicacao, request.DisciplinaId);
        }
    }
}
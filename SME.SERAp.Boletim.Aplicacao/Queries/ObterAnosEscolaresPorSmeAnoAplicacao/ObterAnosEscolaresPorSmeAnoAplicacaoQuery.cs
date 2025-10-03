using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosEscolaresPorSmeAnoAplicacao
{
    public class ObterAnosEscolaresPorSmeAnoAplicacaoQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public int AnoAplicacao { get; set; }
        public int DisciplinaId { get; set; }

        public ObterAnosEscolaresPorSmeAnoAplicacaoQuery(int anoAplicacao, int disciplinaId)
        {
            AnoAplicacao = anoAplicacao;
            DisciplinaId = disciplinaId;
        }
    }
}
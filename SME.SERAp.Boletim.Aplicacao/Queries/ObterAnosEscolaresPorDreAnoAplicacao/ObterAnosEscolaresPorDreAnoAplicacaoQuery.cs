using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnosEscolaresPorDreAnoAplicacaoQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public long DreId { get; set; }

        public int AnoAplicacao { get; set; }

        public int DisciplinaId { get; set; }

        public ObterAnosEscolaresPorDreAnoAplicacaoQuery(long dreId, int anoAplicacao, int disciplinaId)
        {
            DreId = dreId;
            AnoAplicacao = anoAplicacao;
            DisciplinaId = disciplinaId;
        }
    }
}

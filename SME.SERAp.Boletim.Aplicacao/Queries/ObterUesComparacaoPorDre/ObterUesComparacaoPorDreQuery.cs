using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesComparacaoPorDreQuery : IRequest<IEnumerable<UePorDreDto>>
    {
        public long DreId { get; set; }
        public int AnoAplicacao { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
        public ObterUesComparacaoPorDreQuery(long dreId, int anoAplicacao, int disciplinaId, int anoEscolar)
        {
            DreId = dreId;
            AnoAplicacao = anoAplicacao;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }
}

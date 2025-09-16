using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterComponentesCurricularesPorDreAnoQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public long DreId { get; set; }

        public int AnoAplicacao { get; set; }

        public ObterComponentesCurricularesPorDreAnoQuery(long dreId, int anoAplicacao)
        {
            DreId = dreId;
            AnoAplicacao = anoAplicacao;
        }
    }
}

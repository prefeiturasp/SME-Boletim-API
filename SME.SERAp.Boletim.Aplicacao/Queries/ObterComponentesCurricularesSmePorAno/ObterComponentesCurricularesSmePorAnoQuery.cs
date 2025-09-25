using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterComponentesCurricularesSmePorAnoQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public int AnoAplicacao { get; set; }
        public ObterComponentesCurricularesSmePorAnoQuery(int anoAplicacao)
        {
            AnoAplicacao = anoAplicacao;
        }
    }
}

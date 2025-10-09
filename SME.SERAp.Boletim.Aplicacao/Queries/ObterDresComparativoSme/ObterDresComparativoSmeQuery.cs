using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme
{
    public class ObterDresComparativoSmeQuery : IRequest<IEnumerable<DreDto>>
    {
        public ObterDresComparativoSmeQuery(int anoAplicacao, int disciplinaId, int anoEscolar)
        {
            AnoAplicacao = anoAplicacao;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }

        public int AnoAplicacao { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
    }
}

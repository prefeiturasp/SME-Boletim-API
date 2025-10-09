using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSP
{
    public class ObterProficienciaSmeProvaSPQuery : IRequest<IEnumerable<ResultadoProeficienciaSme>>
    {
        public ObterProficienciaSmeProvaSPQuery(int anoLetivo, int disciplinaId, int anoEscolar)
        {
            AnoLetivo = anoLetivo;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
        public int AnoLetivo { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
    }
}

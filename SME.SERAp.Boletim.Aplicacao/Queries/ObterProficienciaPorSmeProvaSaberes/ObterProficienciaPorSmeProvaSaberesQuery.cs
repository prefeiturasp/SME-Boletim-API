using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes
{
    public class ObterProficienciaPorSmeProvaSaberesQuery : IRequest<IEnumerable<ResultadoProeficienciaPorDre>>
    {
        public int AnoLetivo { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }

        public ObterProficienciaPorSmeProvaSaberesQuery(int anoLetivo, int disciplinaId, int anoEscolar)
        {
            AnoLetivo = anoLetivo;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }
}

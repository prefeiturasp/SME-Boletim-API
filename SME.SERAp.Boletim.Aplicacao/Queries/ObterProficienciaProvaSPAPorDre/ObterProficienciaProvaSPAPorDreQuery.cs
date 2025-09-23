using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre
{
    public class ObterProficienciaProvaSPAPorDreQuery : IRequest<ResultadoProeficienciaPorDre>
    {
        public int DreId { get; set; }
        public int AnoLetivo { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }

        public ObterProficienciaProvaSPAPorDreQuery(int dreId, int anoLetivo, int disciplinaId, int anoEscolar)
        {
            DreId = dreId;
            AnoLetivo = anoLetivo;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }

}

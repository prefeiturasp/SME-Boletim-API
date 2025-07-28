using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResumoDresNivelProficienciaDto
    {
        public List<DisciplinaResumoDresNivelProficienciaDto> Disciplinas { get; set; }

        public int AnoEscolar { get; set; }
        public long LoteId { get; set; }
    }
}
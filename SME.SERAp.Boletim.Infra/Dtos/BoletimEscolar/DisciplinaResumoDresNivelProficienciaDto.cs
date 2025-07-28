using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DisciplinaResumoDresNivelProficienciaDto
    {
        public int DisciplinaId { get; set; }
        public string DisciplinaNome { get; set; }

        public List<NivelProficienciaDresDto> DresPorNiveisProficiencia { get; set; }
    }
}
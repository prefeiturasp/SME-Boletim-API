using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciasGraficoComparativoSmeDto
    {
        public string DreAbreviacao { get; set; }
        public string DreNome { get; set; }

        public IEnumerable<ProficienciasGraficoComparativoDreDto> ListaProficienciaGraficoComparativoDto { get; set;}

    }
}

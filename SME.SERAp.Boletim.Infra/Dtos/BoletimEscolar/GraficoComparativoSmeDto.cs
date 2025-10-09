using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class GraficoComparativoSmeDto
    {
        public IEnumerable<string> TodasAplicacoesDisponiveis { get; set; }
        public IEnumerable<ProficienciasGraficoComparativoSmeDto> Dados { get; set; }
    }
}

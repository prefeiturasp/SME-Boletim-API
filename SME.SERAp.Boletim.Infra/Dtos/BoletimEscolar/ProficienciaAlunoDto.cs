using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaAlunoDto
    {
        public string Nome { get; set; }
        public double Variacao { get; set; }
        public IEnumerable<ProficienciaDetalheDto> Proficiencias { get; set; }
    }
}
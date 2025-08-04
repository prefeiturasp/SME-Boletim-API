using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaDreCompletoDto
    {
        public int TotalTipoDisciplina { get; set; }
        public IEnumerable<DreProficienciaDto> Itens { get; set; }
    }
}
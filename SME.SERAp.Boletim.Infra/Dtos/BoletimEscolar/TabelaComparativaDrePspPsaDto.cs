using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class TabelaComparativaDrePspPsaDto
    {
        public decimal Variacao { get; set; }

        public IEnumerable<ProficienciaTabelaComparativaDre> Aplicacao { get; set; }
    }
}

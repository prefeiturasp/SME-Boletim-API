using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaTabelaComparativaDre
    {
        public string Descricao { get; set; }
        public string Mes { get; set; }
        public decimal ValorProficiencia { get; set; }
        public string NivelProficiencia { get; set; }

        public int QtdeUe { get; set; }

        public int QtdeEstudante{ get; set; }
    }
}

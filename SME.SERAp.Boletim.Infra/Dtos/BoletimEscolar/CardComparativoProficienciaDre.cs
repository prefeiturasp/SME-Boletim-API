using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class CardComparativoProficienciaDre
    {
        public string DreAbreviacao { get; set; }
        public string DreNome { get; set; }
        
        public decimal Variacao { get; set; }

        public  ProficienciaDetalheDreDto AplicacaoPsp { get; set; }

        public IEnumerable<ProficienciaDetalheDreDto> AplicacoesPsa { get; set; }

    }
}

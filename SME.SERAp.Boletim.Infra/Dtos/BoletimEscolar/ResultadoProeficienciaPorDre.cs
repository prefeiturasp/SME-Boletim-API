using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResultadoProeficienciaPorDre
    {   public long LoteId { get; set; }
        public string NomeAplicacao { get; set; }

        public string Periodo { get; set; }
        public int QuantidadeAlunosUe { get; set; }

        public int DreId { get; set; }  

        public string DreAbreviacao { get; set; }

        public string DreNome { get; set; }
        public string AnoEscolar { get; set; }
   
        public int DisciplinaId { get; set; }
        public decimal MediaProficiencia { get; set; }
     
        public decimal RealizaramProva { get; set; }
    }
}

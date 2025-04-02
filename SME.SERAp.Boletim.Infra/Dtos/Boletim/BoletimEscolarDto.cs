using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class BoletimEscolarDto
    {
        public long UeId { get; set; }
        public long ProvaId { get; set; }
        public string ComponenteCurricular { get; set; }
        public string AbaixoBasico { get; set; }
        public string Basico { get; set; }
        public string Adequado { get; set; }
        public string Avancado { get; set; }
        public decimal Total { get; set; }

        public decimal MediaProficiencia { get; set; }
        public string DisciplinaDescricao { get; set; }
        public int DisciplinaId { get; set; }

        public string AnoEscolarDescricao { get; set; }

        public int AnoEscolar { get; set;  }

    


    }
}

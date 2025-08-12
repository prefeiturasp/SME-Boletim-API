using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreResumoDto
    {
        public long DreId { get; set; }
        public string DreNome { get; set; }
        public int AnoEscolar { get; set; }
        public int TotalUes { get; set; }
        public int TotalAlunos { get; set; }
        public int TotalRealizaramProva { get; set; }
    }
}
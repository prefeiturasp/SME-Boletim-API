using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class ProficienciaDetalheDreDto
    {
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public decimal MediaProficiencia { get; set; }
        public int RealizaramProva { get; set; }
        public int QuantidadeUes { get; set; }
        public string NivelProficiencia { get; set; }
    }
}

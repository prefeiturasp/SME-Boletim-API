using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaDetalheUeDto
    {
        public long LoteId { get; set; }
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public double MediaProficiencia { get; set; }
        public int RealizaramProva { get; set; }
        public string NivelProficiencia { get; set; }
    }
}
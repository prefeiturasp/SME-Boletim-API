using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaUeDto
    {
        public long LoteId { get; set; }
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public decimal MediaProficiencia { get; set; }
        public string NivelProficiencia { get; set; }
        public int TotalRealizaramProva { get; set; }
    }
}
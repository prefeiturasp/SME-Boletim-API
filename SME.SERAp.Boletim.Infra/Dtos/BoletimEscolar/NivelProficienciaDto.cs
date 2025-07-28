using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class NivelProficienciaDto
    {
        public int DreId { get; set; }
        public string Disciplina { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
        public decimal MediaProficiencia { get; set; }
        public int NivelCodigo { get; set; }
        public string NivelDescricao { get; set; }
    }
}
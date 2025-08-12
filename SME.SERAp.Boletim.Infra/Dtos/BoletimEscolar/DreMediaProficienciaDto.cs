using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreMediaProficienciaDto
    {
        public long DreId { get; set; }
        public string Disciplina { get; set; }
        public long DisciplinaId { get; set; }
        public decimal MediaProficiencia { get; set; }
    }
}
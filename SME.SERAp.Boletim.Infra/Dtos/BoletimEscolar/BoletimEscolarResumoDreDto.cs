using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarResumoDreDto : DtoBase
    {
        public int TotalUes { get; set; }
        public int TotalAlunos { get; set; }
        public IEnumerable<MediaProficienciaDisciplinaDto> ProficienciaDisciplina { get; set; }
    }

    public class MediaProficienciaDisciplinaDto
    {
        public long DisciplinaId { get; set; }
        public string DisciplinaNome { get; set; }
        public decimal MediaProficiencia { get; set; }
    }
}
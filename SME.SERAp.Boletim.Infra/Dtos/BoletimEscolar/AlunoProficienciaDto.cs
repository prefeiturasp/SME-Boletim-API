using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class AlunoProficienciaDto
    {
        public long AlunoRa { get; set; }
        public string NomeAluno { get; set; }
        public string? Raca { get; set; }
        public string? Sexo { get; set; }
        public decimal Proficiencia { get; set; }
        public long LoteId { get; set; }
        public string Turma { get; set; }
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public string DisciplinaNome { get; set; }
        public string NomeLote { get; set; }
    }
}
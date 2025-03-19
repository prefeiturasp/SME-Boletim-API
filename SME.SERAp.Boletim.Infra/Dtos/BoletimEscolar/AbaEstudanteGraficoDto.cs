using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class AbaEstudanteGraficoDto
    {
        public string Turma { get; set; }
        public string Disciplina { get; set; }
        public List<AbaEstudanteGraficoAlunoDto> Alunos { get; set; }
    }

    public class AbaEstudanteGraficoAlunoDto
    {
        public string Nome { get; set; }
        public decimal Proficiencia { get; set; }
    }

    public class AbaEstudanteGraficoTempDto
    {
        public string Turma { get; set; }
        public string Disciplina { get; set; }
        public string Nome { get; set; }
        public decimal Proficiencia { get; set; }
    }

}
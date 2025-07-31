using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DownloadProvasBoletimEscolarPorDreDto : DtoBase
    {
        public long CodigoDre { get; set; }
        public string NomeDreAbreviacao { get; set; }
        public string CodigoUE { get; set; }
        public string NomeUE { get; set; }
        public int AnoEscola { get; set; }
        public string Turma { get; set; }
        public long AlunoRA { get; set; }
        public string NomeAluno { get; set; }
        public string Componente { get; set; }
        public decimal Proficiencia { get; set; }
        public string Nivel { get; set; }
    }
}
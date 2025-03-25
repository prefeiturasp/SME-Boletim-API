using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResultadoProbabilidadeAgrupadoDto
    {
        public string CodigoHabilidade { get; set; }
        public string HabilidadeDescricao { get; set; }
        public IEnumerable<TurmaProbabilidadeDto> Turmas { get; set; }
    }

    public class TurmaProbabilidadeDto
    {
        public string TurmaDescricao { get; set; }
        public decimal? AbaixoDoBasico { get; set; }
        public decimal? Basico { get; set; }
        public decimal? Adequado { get; set; }
        public decimal? Avancado { get; set; }
    }
}
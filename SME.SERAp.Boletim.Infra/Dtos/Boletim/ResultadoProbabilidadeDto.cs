using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class ResultadoProbabilidadeDto
    {
        public string CodigoHabilidade { get; set; }

        public string HabilidadeDescricao { get; set; }

        public string TurmaDescricao { get; set; }

        public decimal? AbaixoDoBasico { get; set; }

        public decimal? Basico { get; set; }

        public decimal? Adequado { get; set; }

        public decimal? Avancado { get; set; }
    }
}
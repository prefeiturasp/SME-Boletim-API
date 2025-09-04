using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaUeComparacaoProvaSPDto
    {
        public int TotalLotes { get; set; }
        public ProficienciaProvaSpDto ProvaSP { get; set; }
        public IEnumerable<ProficienciaUeDto> Lotes { get; set; }
    }
}
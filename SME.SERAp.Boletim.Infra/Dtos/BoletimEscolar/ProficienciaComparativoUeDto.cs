using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaComparativoUeDto
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int ItensPorPagina { get; set; }
        public int DreId { get; set; }
        public string DreAbreviacao { get; set; }
        public IEnumerable<UeProficienciaDto> Ues { get; set; }
    }
}
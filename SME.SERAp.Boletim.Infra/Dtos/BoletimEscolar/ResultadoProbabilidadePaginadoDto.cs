using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResultadoProbabilidadePaginadoDto
    {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<ResultadoProbabilidadeAgrupadoDto> Resultados { get; set; }
    }
}
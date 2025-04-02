using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class PaginacaoDto<T>
    {
        public PaginacaoDto(IEnumerable<T> itens, int pagina, int tamanhoPagina, int totalRegistros)
        {
            Itens = itens;
            Pagina = pagina;
            TamanhoPagina = tamanhoPagina;
            TotalRegistros = totalRegistros;
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);
        }

        public IEnumerable<T> Itens { get; }
        public int Pagina { get; }
        public int TamanhoPagina { get; }
        public int TotalRegistros { get; }
        public int TotalPaginas { get; }
    }
}
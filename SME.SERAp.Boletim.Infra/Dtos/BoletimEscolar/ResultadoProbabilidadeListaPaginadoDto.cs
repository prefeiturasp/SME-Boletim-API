using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResultadoProbabilidadeListaPaginadoDto
    {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<ResultadoProbabilidadeDto> Resultados { get; set; }
    }
}

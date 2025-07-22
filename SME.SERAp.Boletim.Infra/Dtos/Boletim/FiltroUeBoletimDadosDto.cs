namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroUeBoletimDadosDto
    {
        public IEnumerable<long> UesIds { get; set; }

        public int TamanhoPagina { get; set; } = 10;

        public int Pagina { get; set; } = 1;
    }
}

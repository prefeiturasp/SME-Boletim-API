namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroBoletimEstudantePaginadoDto : FiltroBoletimEstudanteDto
    {
        public long EolEstudante { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageNumber { get; set; } = 1;
    }
}

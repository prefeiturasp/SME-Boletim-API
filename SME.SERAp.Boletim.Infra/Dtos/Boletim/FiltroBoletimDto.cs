namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroBoletimDto
    {
        public List<int> Ano { get; set; } = new();
        public List<int> ComponentesCurriculares { get; set; } = new();
    }
}

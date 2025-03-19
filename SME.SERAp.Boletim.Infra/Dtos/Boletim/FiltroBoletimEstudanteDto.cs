namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroBoletimEstudanteDto : FiltroBoletimDto
    {
        public List<int> NivelProficiencia { get; set; } = new();
        public List<string> Turma { get; set; } = new();
        public decimal NivelMinimo { get; set; }
        public decimal NivelMaximo { get; set; }
        public string NomeEstudante { get; set; }
    }
}

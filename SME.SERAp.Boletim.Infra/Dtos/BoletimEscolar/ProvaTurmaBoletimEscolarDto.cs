namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProvaTurmaBoletimEscolarDto : DtoBase
    {
        public string Turma { get; set; }
        public string AbaixoBasico { get; set; }
        public string Basico { get; set; }
        public string Adequado { get; set; }
        public string Avancado { get; set; }
        public decimal Total { get; set; }
        public decimal MediaProficiencia { get; set; }
    }
}

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class TurmaBoletimEscolarDto : DtoBase
    {
        public string Turma { get; set; }
        public decimal AbaixoBasico { get; set; }
        public decimal AbaixoBasicoPorcentagem { get; set; }
        public decimal Basico { get; set; }
        public decimal BasicoPorcentagem { get; set; }
        public decimal Adequado { get; set; }
        public decimal AdequadoPorcentagem { get; set; }
        public decimal Avancado { get; set; }
        public decimal AvancadoPorcentagem { get; set; }
        public decimal Total { get; set; }
        public decimal MediaProficiencia { get; set; }
    }
}

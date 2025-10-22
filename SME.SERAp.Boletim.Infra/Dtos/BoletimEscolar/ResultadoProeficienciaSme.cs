namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ResultadoProeficienciaSme
    {
        public long LoteId { get; set; }
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public int QuantidadeDres { get; set; }
        public int QuantidadeUes { get; set; }
        public string AnoEscolar { get; set; }
        public int DisciplinaId { get; set; }
        public decimal MediaProficiencia { get; set; }
        public int RealizaramProva { get; set; }
    }
}

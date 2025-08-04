namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DownloadResultadoProbabilidadeDto
    {
        public string CodigoDre { get; set; }
        public string NomeDreAbreviacao { get; set; }
        public string CodigoUe { get; set; }
        public string NomeUe { get; set; }
        public int AnoEscolar { get; set; }
        public string Componente { get; set; }
        public string CodigoHabilidade { get; set; }
        public string HabilidadeDescricao { get; set; }
        public string TurmaDescricao { get; set; }
        public decimal? AbaixoDoBasico { get; set; }
        public decimal? Basico { get; set; }
        public decimal? Adequado { get; set; }
        public decimal? Avancado { get; set; }
    }
}
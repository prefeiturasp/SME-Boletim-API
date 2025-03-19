namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DownloadProvasBoletimEscolarDto : DtoBase
    {
        public string CodigoUE { get; set; }
        public string NomeUE { get; set; }
        public int AnoEscola { get; set; }
        public string Turma { get; set; }
        public int AlunoRA { get; set; }
        public string NomeAluno { get; set; }
        public string Componente { get; set; }
        public decimal Proficiencia { get; set; }
        public string Nivel { get; set; }
    }
}

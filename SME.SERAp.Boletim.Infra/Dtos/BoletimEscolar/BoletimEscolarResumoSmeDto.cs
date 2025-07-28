namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarResumoSmeDto
    {
        public int TotalDres { get; set; }
        public int TotalUes { get; set; }
        public int TotalAlunos { get; set; }
        public IEnumerable<MediaProficienciaDisciplinaDto> ProficienciaDisciplina { get; set; }
    }
}

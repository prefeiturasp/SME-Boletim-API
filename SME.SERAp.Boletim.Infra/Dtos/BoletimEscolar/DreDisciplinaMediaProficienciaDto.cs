namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreDisciplinaMediaProficienciaDto
    {
        public long DreId { get; set; }

        public string DreNome { get; set; }

        public int DisciplinaId { get; set; }

        public string Disciplina { get; set; }

        public long ProvaId { get; set; }

        public decimal MediaProficiencia { get; set; }  
    }
}

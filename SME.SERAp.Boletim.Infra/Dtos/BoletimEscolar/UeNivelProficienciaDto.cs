namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeNivelProficienciaDto
    {
        public string Codigo { get; set; }

        public string Nome { get; set; }

        public string Disciplina { get; set; }

        public int DisciplinaId { get; set; }

        public int AnoEscolar { get; set; }

        public decimal MediaProficiencia { get; set; }

        public int NivelCodigo { get; set; }

        public string NivelDescricao { get; set; }
    }
}

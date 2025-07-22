namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeBoletimDisciplinaProficienciaDto
    {
        public long UeId { get; set; }

        public int DisciplinaId { get; set; }

        public string Disciplina { get; set; }

        public decimal MediaProficiencia { get; set; }

        public int NivelCodigo { get; set; }

        public string NivelDescricao { get; set; }
    }
}

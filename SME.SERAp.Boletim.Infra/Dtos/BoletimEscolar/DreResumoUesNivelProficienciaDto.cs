namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreResumoUesNivelProficienciaDto
    {
        public List<DisciplinaResumoUesNivelProficienciaDto> Disciplinas { get; set; }

        public int AnoEscolar { get; set; }

        public long LoteId { get; set; }

        public long DreId { get; set; }
    }
}

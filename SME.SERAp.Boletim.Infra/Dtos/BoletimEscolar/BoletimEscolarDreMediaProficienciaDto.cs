namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarDreMediaProficienciaDto
    {
        public string DreNome { get; set; }

        public long DreId { get; set; }

        public IEnumerable<DisciplinaMediaProficienciaDto> Diciplinas { get; set; }
    }
}

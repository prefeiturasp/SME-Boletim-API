namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DisciplinaResumoUesNivelProficienciaDto
    {
        public int DisciplinaId { get;set; }

        public string DisciplinaNome { get;set; }

        public List<NivelProficienciaUesDto> UesPorNiveisProficiencia { get;set; }
    }
}

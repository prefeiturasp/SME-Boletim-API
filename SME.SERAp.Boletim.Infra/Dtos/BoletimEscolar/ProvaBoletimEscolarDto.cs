namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProvaBoletimEscolarDto : DtoBase
    {
        public long Id { get; set; }

        public string Descricao { get; set; }

        public IEnumerable<ProvaNivelProficienciaBoletimEscolarDto> Niveis { get; set; }

        public IEnumerable<TurmaBoletimEscolarDto> Turmas { get; set; }
    }
}

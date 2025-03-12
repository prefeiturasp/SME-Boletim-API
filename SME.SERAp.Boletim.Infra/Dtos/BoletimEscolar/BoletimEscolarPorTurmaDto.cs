namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarPorTurmaDto : DtoBase
    {
        public BoletimEscolarPorTurmaDto()
        {

        }

        public BoletimEscolarPorTurmaDto(IEnumerable<ProvaBoletimEscolarDto> provas) : this()
        {
            Provas = provas;
        }

        public IEnumerable<ProvaBoletimEscolarDto> Provas { get; set; }
    }
}

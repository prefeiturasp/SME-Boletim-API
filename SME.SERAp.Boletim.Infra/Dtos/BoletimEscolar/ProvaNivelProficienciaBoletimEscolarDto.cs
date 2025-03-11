namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProvaNivelProficienciaBoletimEscolarDto : DtoBase
    {
        public string AnoEscolar { get; set; }

        public string AbaixoBasico { get; set; }

        public string Basico { get; set; }

        public string Adequado { get; set; }

        public string Avancado { get; set; }
    }
}

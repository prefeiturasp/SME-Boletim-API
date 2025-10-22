namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class TabelaComparativaSmePspPsaDto
    {
        public decimal Variacao { get; set; }

        public IEnumerable<ProficienciaTabelaComparativaSmeDto> Aplicacao { get; set; }
    }
}

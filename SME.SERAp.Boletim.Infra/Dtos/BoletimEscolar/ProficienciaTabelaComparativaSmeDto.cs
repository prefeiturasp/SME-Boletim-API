namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaTabelaComparativaSmeDto
    {
        public string Descricao { get; set; }
        public string Mes { get; set; }
        public decimal ValorProficiencia { get; set; }
        public string NivelProficiencia { get; set; }
        public int QtdeUe { get; set; }
        public int QtdeDre { get; set; }
        public int QtdeEstudante { get; set; }
    }
}

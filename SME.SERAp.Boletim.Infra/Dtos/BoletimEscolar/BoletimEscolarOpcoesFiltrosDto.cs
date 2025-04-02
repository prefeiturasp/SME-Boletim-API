namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarOpcoesFiltrosDto
    {
        public IEnumerable<OpcaoFiltroDto<int>> Niveis { get; set; }
        public IEnumerable<OpcaoFiltroDto<int>> AnosEscolares { get; set; }
        public IEnumerable<OpcaoFiltroDto<int>> ComponentesCurriculares { get; set; }
        public IEnumerable<OpcaoFiltroDto<string>> Turmas { get; set; }
        public decimal NivelMinimo { get; set; }
        public decimal NivelMaximo { get; set; }
    }
}

namespace SME.SERAp.Boletim.Infra.Dtos.LoteProva
{
    public class LoteProvaDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public bool TipoTai { get; set; }

        public bool ExibirNoBoletim { get; set; }

        public DateTime DataInicioLote { get; set; }
    }
}

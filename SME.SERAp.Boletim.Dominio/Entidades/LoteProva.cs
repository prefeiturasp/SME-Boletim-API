namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class LoteProva : EntidadeBase
    {
        public string Nome { get; set; }
        public bool TipoTai { get; set; }
        public bool ExibirNoBoletim { get; set; }
        public DateTime DataCorrecaoFim { get; set; }
        public DateTime DataInicioLote { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}

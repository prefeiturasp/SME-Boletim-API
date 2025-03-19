namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class OpcaoFiltroDto<TValor>
    {
        public string Texto { get; set; }
        public TValor Valor { get; set; }
    }
}

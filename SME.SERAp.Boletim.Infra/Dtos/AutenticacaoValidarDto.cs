namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AutenticacaoValidarDto
    {
        public AutenticacaoValidarDto(string codigo)
        {
            Codigo = codigo;
        }

        public string Codigo { get; set; }
    }
}

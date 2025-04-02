namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AutenticacaoRetornoDto
    {
        public AutenticacaoRetornoDto(string token, DateTime dataHoraExpiracao)
        {
            Token = token;
            DataHoraExpiracao = dataHoraExpiracao;
        }

        public string Token { get; set; }
        public DateTime DataHoraExpiracao { get; set; }
    }
}

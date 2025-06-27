using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AutenticacaoRetornoDto
    {
        public AutenticacaoRetornoDto(string token, DateTime dataHoraExpiracao, TipoPerfil tipoPerfil)
        {
            Token = token;
            DataHoraExpiracao = dataHoraExpiracao;
            TipoPerfil = tipoPerfil;
        }

        public string Token { get; set; }
        public DateTime DataHoraExpiracao { get; set; }
        public TipoPerfil TipoPerfil { get; set; }
    }
}

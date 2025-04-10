namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AutenticacaoDto
    {
        public string Login { get; set; }
        public Guid Perfil { get; set; }
        public string ChaveApi { get; set; }
    }
}
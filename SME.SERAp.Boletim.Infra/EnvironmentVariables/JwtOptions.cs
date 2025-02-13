namespace SME.SERAp.Boletim.Infra.EnvironmentVariables
{
    public class JwtOptions
    {
        public static string Secao => "JwtOptions";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ExpiresInMinutes { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}

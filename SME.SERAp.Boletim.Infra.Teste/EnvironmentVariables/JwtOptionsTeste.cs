using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class JwtOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var issuer = "serap-api";
            var audience = "serap-client";
            var expiresInMinutes = "60";
            var issuerSigningKey = "chave-super-secreta";

            var options = new JwtOptions
            {
                Issuer = issuer,
                Audience = audience,
                ExpiresInMinutes = expiresInMinutes,
                IssuerSigningKey = issuerSigningKey
            };

            Assert.Equal(issuer, options.Issuer);
            Assert.Equal(audience, options.Audience);
            Assert.Equal(expiresInMinutes, options.ExpiresInMinutes);
            Assert.Equal(issuerSigningKey, options.IssuerSigningKey);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new JwtOptions
            {
                Issuer = null,
                Audience = null,
                ExpiresInMinutes = null,
                IssuerSigningKey = null
            };

            Assert.Null(options.Issuer);
            Assert.Null(options.Audience);
            Assert.Null(options.ExpiresInMinutes);
            Assert.Null(options.IssuerSigningKey);
        }

        [Fact]
        public void Secao_Deve_Retornar_Jwt()
        {
            Assert.Equal("Jwt", JwtOptions.Secao);
        }
    }
}

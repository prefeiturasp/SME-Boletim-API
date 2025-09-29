using SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.IdentityModel.Tokens.Jwt;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTokenJwtQueryHandlerTeste
    {
        private readonly JwtOptions jwtOptions;
        private readonly ObterTokenJwtQueryHandler handler;

        public ObterTokenJwtQueryHandlerTeste()
        {
            jwtOptions = new JwtOptions
            {
                Issuer = "meu-issuer",
                Audience = "minha-audience",
                ExpiresInMinutes = "60",
                IssuerSigningKey = "uma-chave-muito-secreta-1234567890"
            };

            handler = new ObterTokenJwtQueryHandler(jwtOptions);
        }

        [Fact]
        public async Task Deve_Gerar_Token_Valido_Para_Perfil_Comum_Com_Claims_Corretos()
        {
            var perfilComum = Guid.NewGuid();
            var abrangencias = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto
                {
                    Login = "usuario1",
                    UsuarioId = 123,
                    Usuario = "Usuario Teste",
                    GrupoId = 1,
                    Grupo = "Grupo Teste",
                    Perfil = perfilComum,
                    DreId = 10,
                    UeId = 20,
                    TurmaId = 30
                }
            };

            var query = new ObterTokenJwtQuery(abrangencias);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado.Token);
            Assert.True(resultado.DataHoraExpiracao > DateTime.Now);
            Assert.Equal(Perfis.ObterTipoPerfil(perfilComum), resultado.TipoPerfil);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(resultado.Token);

            Assert.Contains(token.Claims, c => c.Type == "LOGIN" && c.Value == "usuario1");
            Assert.Contains(token.Claims, c => c.Type == "USUARIOID" && c.Value == "123");
            Assert.Contains(token.Claims, c => c.Type == "USUARIO" && c.Value == "Usuario Teste");
            Assert.Contains(token.Claims, c => c.Type == "GRUPOID" && c.Value == "1");
            Assert.Contains(token.Claims, c => c.Type == "GRUPO" && c.Value == "Grupo Teste");
            Assert.Contains(token.Claims, c => c.Type == "PERFIL" && c.Value == perfilComum.ToString());
            Assert.Contains(token.Claims, c => c.Type == "DRE-UE-TURMA" && c.Value == "10-20-30");
        }

        [Fact]
        public async Task Deve_Gerar_Token_Valido_Para_Perfil_Administrador_Sem_Claims_DRE_UE_Turma()
        {
            var perfilAdmin = Perfis.PERFIL_ADMINISTRADOR;
            var abrangencias = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto
                {
                    Login = "admin",
                    UsuarioId = 1,
                    Usuario = "Admin",
                    GrupoId = 1,
                    Grupo = "Administradores",
                    Perfil = perfilAdmin,
                    DreId = 0,
                    UeId = null,
                    TurmaId = 0
                }
            };

            var query = new ObterTokenJwtQuery(abrangencias);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado.Token);
            Assert.True(resultado.DataHoraExpiracao > DateTime.Now);
            Assert.Equal(Perfis.ObterTipoPerfil(perfilAdmin), resultado.TipoPerfil);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(resultado.Token);

            Assert.Contains(token.Claims, c => c.Type == "LOGIN" && c.Value == "admin");
            Assert.Contains(token.Claims, c => c.Type == "USUARIOID" && c.Value == "1");
            Assert.Contains(token.Claims, c => c.Type == "USUARIO" && c.Value == "Admin");
            Assert.Contains(token.Claims, c => c.Type == "GRUPOID" && c.Value == "1");
            Assert.Contains(token.Claims, c => c.Type == "GRUPO" && c.Value == "Administradores");
            Assert.Contains(token.Claims, c => c.Type == "PERFIL" && c.Value == perfilAdmin.ToString());
            Assert.DoesNotContain(token.Claims, c => c.Type == "DRE-UE-TURMA");
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_JwtOptions_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTokenJwtQueryHandler(null));
        }
    }
}

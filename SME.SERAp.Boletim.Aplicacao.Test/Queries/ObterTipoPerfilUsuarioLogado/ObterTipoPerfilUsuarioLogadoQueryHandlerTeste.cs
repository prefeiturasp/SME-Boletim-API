using Microsoft.AspNetCore.Http;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTipoPerfilUsuarioLogadoQueryHandlerTeste
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly ObterTipoPerfilUsuarioLogadoQueryHandler obterTipoPerfilUsuarioLogadoQueryHandler;
        public ObterTipoPerfilUsuarioLogadoQueryHandlerTeste()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            obterTipoPerfilUsuarioLogadoQueryHandler = new ObterTipoPerfilUsuarioLogadoQueryHandler(httpContextAccessor.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Tipo_Perfil_Professor()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", Perfis.PERFIL_PROFESSOR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(TipoPerfil.Professor, resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Tipo_Perfil_Coordenador()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", Perfis.PERFIL_COORDENADOR_PEDAGOGICO.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(TipoPerfil.Coordenador, resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Tipo_Perfil_Diretor()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", Perfis.PERFIL_DIRETOR_ESCOLAR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(TipoPerfil.Diretor, resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Tipo_Perfil_Administrador_DRE()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", Perfis.PERFIL_ADMINISTRADOR_DRE.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(TipoPerfil.Administrador_DRE, resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Tipo_Perfil_Administrador()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", Perfis.PERFIL_ADMINISTRADOR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(TipoPerfil.Administrador, resultado);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Tipo_Perfil()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("PERFIL", string.Empty)
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var resultado = await obterTipoPerfilUsuarioLogadoQueryHandler.Handle(new ObterTipoPerfilUsuarioLogadoQuery(), CancellationToken.None);

            Assert.Null(resultado);
        }
    }
}

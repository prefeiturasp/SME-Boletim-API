using Microsoft.AspNetCore.Http;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Cache;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using System.Security.Claims;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDresAbrangenciaUsuarioLogadoQueryHandlerTeste
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly ObterDresAbrangenciaUsuarioLogadoQueryHandler obterDresAbrangenciaUsuarioLogadoQueryHandler;

        public ObterDresAbrangenciaUsuarioLogadoQueryHandlerTeste()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            repositorioCache = new Mock<IRepositorioCache>();
            obterDresAbrangenciaUsuarioLogadoQueryHandler = new ObterDresAbrangenciaUsuarioLogadoQueryHandler(
                httpContextAccessor.Object,
                repositorioAbrangencia.Object,
                repositorioCache.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Abrangencia_Do_Usuario_Logado_Nao_Administrador()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("LOGIN", "123"),
                new Claim("PERFIL", Perfis.PERFIL_PROFESSOR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            repositorioCache.Setup(x => x.ObterRedisToJsonAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);


            var dresAbrangenciaAdministrador = ObterDresAbragenciaUsuarioAdministrador();
            var dresAbrangenciaNaoAdministrador = ObterDresAbragenciaUsuarioNaoAdministrador();

            repositorioAbrangencia.Setup(r => r.ObterDresAdministrador()).ReturnsAsync(dresAbrangenciaAdministrador);
            repositorioAbrangencia.Setup(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(dresAbrangenciaNaoAdministrador);

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var cancellationToken = CancellationToken.None;
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, cancellationToken);
            Assert.NotNull(resultado);
            Assert.Equal(dresAbrangenciaNaoAdministrador.Count, resultado.Count());
            Assert.Contains(resultado, x => dresAbrangenciaNaoAdministrador.Any(d => d.Id == x.Id));
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Once);
            repositorioCache.Verify(r => r.SalvarRedisToJsonAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Abrangencia_Do_Usuario_Logado_Administrador()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("LOGIN", "321"),
                new Claim("PERFIL", Perfis.PERFIL_ADMINISTRADOR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            repositorioCache.Setup(x => x.ObterRedisToJsonAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            var dresAbrangenciaAdministrador = ObterDresAbragenciaUsuarioAdministrador();
            var dresAbrangenciaNaoAdministrador = ObterDresAbragenciaUsuarioNaoAdministrador();

            repositorioAbrangencia.Setup(r => r.ObterDresAdministrador()).ReturnsAsync(dresAbrangenciaAdministrador);
            repositorioAbrangencia.Setup(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(dresAbrangenciaNaoAdministrador);

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var cancellationToken = CancellationToken.None;
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, cancellationToken);

            Assert.NotNull(resultado);
            Assert.Equal(dresAbrangenciaAdministrador.Count, resultado.Count());
            Assert.Contains(resultado, x=> dresAbrangenciaAdministrador.Any(d => d.Id == x.Id));
            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Once);
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Once);
            repositorioCache.Verify(r => r.SalvarRedisToJsonAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dres_Abrangencia_Do_Usuario_Logado()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("LOGIN", "321"),
                new Claim("PERFIL", Perfis.PERFIL_ADMINISTRADOR.ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            repositorioCache.Setup(x => x.ObterRedisToJsonAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            repositorioAbrangencia.Setup(r => r.ObterDresAdministrador()).ReturnsAsync(new List<DreAbragenciaDetalheDto>());
            repositorioAbrangencia.Setup(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<DreAbragenciaDetalheDto>());

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var cancellationToken = CancellationToken.None;
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, cancellationToken);

            Assert.Empty(resultado);
            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Once);
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Once);
            repositorioCache.Verify(r => r.SalvarRedisToJsonAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dres_Abrangencia_Quando_Login_Usuario_For_Nulo()
        {
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal());

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, CancellationToken.None);

            Assert.Null(resultado);
            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Never);
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dres_Abrangencia_Quando_Perfil_Usuario_For_Invalido()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("LOGIN", "123"),
                new Claim("PERFIL", "PerfilInvalido")
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, CancellationToken.None);

            Assert.Null(resultado);
            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Never);
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Abrangencia_Do_Cache()
        {
            var claimsUsuarioLogado = new List<Claim>
            {
                new Claim("LOGIN", "123"),
                new Claim("PERFIL", Guid.NewGuid().ToString())
            };

            var identityUsuarioLogado = new ClaimsIdentity(claimsUsuarioLogado, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identityUsuarioLogado);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var dresCache = new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 99, Nome = "DRE cacheada" }
            };

            repositorioCache.Setup(r => r.ObterRedisToJsonAsync(It.IsAny<string>()))
                .ReturnsAsync(JsonSerializer.Serialize(dresCache));

            var requisicao = new ObterDresAbrangenciaUsuarioLogadoQuery();
            var resultado = await obterDresAbrangenciaUsuarioLogadoQueryHandler.Handle(requisicao, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(99, resultado.First().Id);

            repositorioAbrangencia.Verify(r => r.ObterDresAdministrador(), Times.Never);
            repositorioAbrangencia.Verify(r => r.ObterDresAbrangenciaPorLoginPerfil(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
            repositorioCache.Verify(r => r.ObterRedisToJsonAsync(It.IsAny<string>()), Times.Once);
            repositorioCache.Verify(r => r.SalvarRedisToJsonAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        private static List<DreAbragenciaDetalheDto> ObterDresAbragenciaUsuarioAdministrador()
        {
            return new List<DreAbragenciaDetalheDto> 
            { 
                new DreAbragenciaDetalheDto { Id = 1, Nome = "DRE admin" },
                new DreAbragenciaDetalheDto { Id = 2, Nome = "DRE admin 2"}
            };
        }

        private static List<DreAbragenciaDetalheDto> ObterDresAbragenciaUsuarioNaoAdministrador()
        {
            return new List<DreAbragenciaDetalheDto> 
            {
                new DreAbragenciaDetalheDto { Id = 3, Nome = "DRE não admin 3"},
                new DreAbragenciaDetalheDto { Id = 4, Nome = "DRE não admin 4"}
            };
        }
    }
}

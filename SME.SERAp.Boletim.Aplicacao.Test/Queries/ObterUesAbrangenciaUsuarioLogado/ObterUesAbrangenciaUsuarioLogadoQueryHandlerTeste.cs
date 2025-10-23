using Microsoft.AspNetCore.Http;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using System.Security.Claims;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterUesAbrangenciaUsuarioLogadoQueryHandlerTeste
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly ObterUesAbrangenciaUsuarioLogadoQueryHandler handler;

        public ObterUesAbrangenciaUsuarioLogadoQueryHandlerTeste()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            repositorioCache = new Mock<IRepositorioCache>();
            handler = new ObterUesAbrangenciaUsuarioLogadoQueryHandler(httpContextAccessor.Object, repositorioAbrangencia.Object, repositorioCache.Object);
        }

        private void ConfigurarHttpContext(string login, string perfil = null)
        {
            var claims = new List<Claim> { new Claim("LOGIN", login) };
            if (perfil != null)
                claims.Add(new Claim("PERFIL", perfil));
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };
            httpContextAccessor.Setup(x => x.HttpContext).Returns(context);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Do_Cache_Quando_Existirem()
        {
            var login = "usuario123";
            ConfigurarHttpContext(login, Perfis.PERFIL_ADMINISTRADOR.ToString());
            var uesEsperadas = new List<AbrangenciaUeDto> { new AbrangenciaUeDto { UeId = 1, UeNome = "UE A" }, new AbrangenciaUeDto { UeId = 2, UeNome = "UE B" } };
            repositorioCache.Setup(r => r.ObterRedisAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>())).ReturnsAsync(uesEsperadas);
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            repositorioCache.Verify(r => r.ObterRedisAsync(It.Is<string>(k => k.Contains(login)), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Nao_Houver_Login()
        {
            httpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Perfil_Nulo()
        {
            ConfigurarHttpContext("usuario123");
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Perfil_Invalido()
        {
            ConfigurarHttpContext("usuario123", "invalido");
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Do_Repositorio_Quando_Perfil_Administrador()
        {
            var login = "admin";
            var esperado = new List<AbrangenciaUeDto> { new AbrangenciaUeDto { UeId = 1, UeNome = "UE Admin" } };
            ConfigurarHttpContext(login, Perfis.PERFIL_ADMINISTRADOR.ToString());
            repositorioAbrangencia.Setup(r => r.ObterUesAdministrador()).ReturnsAsync(esperado);
            repositorioCache.Setup(r => r.ObterRedisAsync<IEnumerable<AbrangenciaUeDto>>(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>())).ReturnsAsync(esperado);
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Single(resultado);
            Assert.Equal("UE Admin", resultado.First().UeNome);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Do_Repositorio_Quando_Nao_Administrador()
        {
            var login = "usuario123";
            ConfigurarHttpContext(login, Perfis.PERFIL_PROFESSOR.ToString());
            var abrangencias = new List<AbrangenciaDetalheDto> { new AbrangenciaDetalheDto { DreId = 1, UeId = 2 } };
            var uesEsperadas = new List<AbrangenciaUeDto> { new AbrangenciaUeDto { UeId = 2, UeNome = "UE X" } };
            repositorioAbrangencia.Setup(r => r.ObterAbrangenciaPorLogin(login)).ReturnsAsync(abrangencias);
            repositorioAbrangencia.Setup(r => r.ObterUesPorAbrangenciaDre(1, 2)).ReturnsAsync(uesEsperadas);
            repositorioCache.Setup(r => r.ObterRedisAsync<IEnumerable<AbrangenciaUeDto>>(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>())).ReturnsAsync(uesEsperadas);
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Single(resultado);
            Assert.Equal("UE X", resultado.First().UeNome);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Nao_Tiver_Abrangencia()
        {
            var login = "usuario123";
            ConfigurarHttpContext(login, Perfis.PERFIL_PROFESSOR.ToString());
            repositorioAbrangencia.Setup(r => r.ObterAbrangenciaPorLogin(login)).ReturnsAsync(new List<AbrangenciaDetalheDto>());
            repositorioCache.Setup(r => r.ObterRedisAsync<IEnumerable<AbrangenciaUeDto>>(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>())).ReturnsAsync((List<AbrangenciaUeDto>)null!);
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            var resultado = await handler.Handle(requisicao, CancellationToken.None);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var login = "usuario123";
            ConfigurarHttpContext(login, Perfis.PERFIL_ADMINISTRADOR.ToString());
            repositorioCache.Setup(r => r.ObterRedisAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<AbrangenciaUeDto>>>>(), It.IsAny<int>())).ThrowsAsync(new Exception("Erro no cache"));
            var requisicao = new ObterUesAbrangenciaUsuarioLogadoQuery();
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(requisicao, CancellationToken.None));
        }
    }
}

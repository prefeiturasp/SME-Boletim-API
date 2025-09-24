using MessagePack;
using Moq;
using SME.SERAp.Boletim.Dados.Cache;
using SME.SERAp.Boletim.Infra.Interfaces;
using StackExchange.Redis;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios
{
    public class RepositorioCacheTeste
    {
        private readonly Mock<IServicoLog> servicoLog;
        private readonly Mock<IDatabase> database;
        private readonly Mock<IConnectionMultiplexer> connection;
        private readonly RepositorioCache repositorio;

        public RepositorioCacheTeste()
        {
            servicoLog = new Mock<IServicoLog>();
            database = new Mock<IDatabase>();
            connection = new Mock<IConnectionMultiplexer>();

            connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                           .Returns(database.Object);

            repositorio = new RepositorioCache(servicoLog.Object, connection.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_ServicoLog_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RepositorioCache(null, connection.Object));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Connection_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RepositorioCache(servicoLog.Object, null));
        }

        [Fact]
        public async Task SalvarRedisAsync_Deve_Salvar_Quando_Valor_Valido()
        {
            var obj = new { Nome = "Mario" };

            await repositorio.SalvarRedisAsync("chave", obj);

            database.Verify(d => d.StringSetAsync(
                "chave",
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                false, When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task SalvarRedisAsync_Nao_Deve_Salvar_Quando_Valor_Nulo()
        {
            await repositorio.SalvarRedisAsync("chave", null);

            database.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()), Times.Never);
        }

        [Fact]
        public async Task SalvarRedisAsync_Deve_Logar_Excecao()
        {
            database.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), false, When.Always, CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            await repositorio.SalvarRedisAsync("chave", "valor");

            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task RemoverRedisAsync_Deve_Remover()
        {
            await repositorio.RemoverRedisAsync("chave");

            database.Verify(d => d.KeyDeleteAsync("chave", CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task RemoverRedisAsync_Deve_Logar_Excecao()
        {
            database.Setup(d => d.KeyDeleteAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            await repositorio.RemoverRedisAsync("chave");

            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisAsync_Com_Busca_Deve_Retornar_Do_Cache()
        {
            var esperado = new { Nome = "Mario" };
            var bytes = MessagePackSerializer.Serialize(esperado);

            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync(bytes);

            var result = await repositorio.ObterRedisAsync("chave", () => Task.FromResult(esperado));

            Assert.Equal(esperado.Nome, result.Nome);
        }

        [Fact]
        public async Task ObterRedisAsync_Com_Busca_Deve_Buscar_Quando_Nao_Tem_Cache()
        {
            var esperado = new { Nome = "Mario" };

            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync((byte[])null);

            var result = await repositorio.ObterRedisAsync("chave", () => Task.FromResult(esperado));

            Assert.Equal(esperado.Nome, result.Nome);
            database.Verify(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), false, When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task ObterRedisAsync_Com_Busca_Deve_Logar_Excecao_E_Chamar_BuscarDados()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            var result = await repositorio.ObterRedisAsync("chave", () => Task.FromResult("fallback"));

            Assert.Equal("fallback", result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisAsync_Sem_Busca_Deve_Retornar_Do_Cache()
        {
            var esperado = "Mario";
            var bytes = MessagePackSerializer.Serialize(esperado);

            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync(bytes);

            var result = await repositorio.ObterRedisAsync<string>("chave");

            Assert.Equal(esperado, result);
        }

        [Fact]
        public async Task ObterRedisAsync_Sem_Busca_Deve_Logar_Excecao()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            var result = await repositorio.ObterRedisAsync<string>("chave");

            Assert.Null(result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisAsync_Deve_Retornar_Default()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                 .ReturnsAsync((byte[])null);

            var result = await repositorio.ObterRedisAsync<string>("chave");

            Assert.Equal(default, result);
        }

        [Fact]
        public async Task ExisteChaveAsync_Deve_Retornar_True()
        {
            database.Setup(d => d.KeyExistsAsync("chave", CommandFlags.None))
                .ReturnsAsync(true);

            var result = await repositorio.ExisteChaveAsync("chave");

            Assert.True(result);
        }

        [Fact]
        public async Task ExisteChaveAsync_Deve_Logar_Excecao()
        {
            database.Setup(d => d.KeyExistsAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            var result = await repositorio.ExisteChaveAsync("chave");

            Assert.False(result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task SalvarRedisToJsonAsync_Deve_Salvar_Json()
        {
            var json = "{\"nome\":\"Mario\"}";

            await repositorio.SalvarRedisToJsonAsync("chave", json);

            database.Verify(d => d.StringSetAsync("chave",
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                false, When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task SalvarRedisToJsonAsync_Nao_Deve_Salvar_Json_Vazio()
        {
            await repositorio.SalvarRedisToJsonAsync("chave", "");

            database.Verify(d => d.StringSetAsync(It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                false, When.Always, CommandFlags.None), Times.Never);
        }

        [Fact]
        public async Task SalvarRedisToJsonAsync_Deve_Logar_Excecao()
        {
            database.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), false, When.Always, CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            await repositorio.SalvarRedisToJsonAsync("chave", "{\"nome\":\"Mario\"}");

            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Com_Busca_Deve_Retornar_Do_Cache()
        {
            var obj = new { Nome = "Mario" };
            var bytes = MessagePackSerializer.Serialize(obj);

            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync(bytes);

            var result = await repositorio.ObterRedisToJsonAsync("chave", () => Task.FromResult("{}"));

            Assert.Contains("Mario", result);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Com_Busca_Deve_Buscar_Quando_Nao_Tem_Cache()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync((byte[])null);

            var result = await repositorio.ObterRedisToJsonAsync("chave", () => Task.FromResult("{\"nome\":\"Mario\"}"));

            Assert.Contains("Mario", result);
            database.Verify(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), false, When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Com_Busca_Deve_Logar_Excecao()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            var result = await repositorio.ObterRedisToJsonAsync("chave", () => Task.FromResult("{\"nome\":\"Mario\"}"));

            Assert.Contains("Mario", result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Sem_Busca_Deve_Retornar_Do_Cache()
        {
            var obj = new { Nome = "Mario" };
            var bytes = MessagePackSerializer.Serialize(obj);

            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync(bytes);

            var result = await repositorio.ObterRedisToJsonAsync("chave");

            Assert.Contains("Mario", result);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Sem_Busca_Deve_Logar_Excecao()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ThrowsAsync(new Exception("erro"));

            var result = await repositorio.ObterRedisToJsonAsync("chave");

            Assert.Null(result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ObterRedisToJsonAsync_Deve_Retornar_Default()
        {
            database.Setup(d => d.StringGetAsync("chave", CommandFlags.None))
                .ReturnsAsync((byte[])null);

            var result = await repositorio.ObterRedisToJsonAsync("chave");

            Assert.Equal(default, result);
        }
    }
}

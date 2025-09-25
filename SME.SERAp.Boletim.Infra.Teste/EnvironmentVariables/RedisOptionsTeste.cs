using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using StackExchange.Redis;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class RedisOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var endpoint = "localhost:6379";
            var proxy = Proxy.Twemproxy;
            var syncTimeout = 10000;

            var options = new RedisOptions
            {
                Endpoint = endpoint,
                Proxy = proxy,
                SyncTimeout = syncTimeout
            };

            Assert.Equal(endpoint, options.Endpoint);
            Assert.Equal(proxy, options.Proxy);
            Assert.Equal(syncTimeout, options.SyncTimeout);
        }

        [Fact]
        public void Deve_Usar_Valor_Padrao_Para_SyncTimeout()
        {
            var options = new RedisOptions();

            Assert.Equal(5000, options.SyncTimeout);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new RedisOptions
            {
                Endpoint = null,
                Proxy = Proxy.None
            };

            Assert.Null(options.Endpoint);
            Assert.Equal(Proxy.None, options.Proxy);
        }

        [Fact]
        public void Secao_Deve_Retornar_Redis()
        {
            Assert.Equal("Redis", RedisOptions.Secao);
        }
    }
}

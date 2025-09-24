using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class RabbitLogOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var hostName = "localhost";
            var userName = "guest";
            var password = "guest";
            var virtualHost = "/";

            var options = new RabbitLogOptions
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            };

            Assert.Equal(hostName, options.HostName);
            Assert.Equal(userName, options.UserName);
            Assert.Equal(password, options.Password);
            Assert.Equal(virtualHost, options.VirtualHost);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new RabbitLogOptions
            {
                HostName = null,
                UserName = null,
                Password = null,
                VirtualHost = null
            };

            Assert.Null(options.HostName);
            Assert.Null(options.UserName);
            Assert.Null(options.Password);
            Assert.Null(options.VirtualHost);
        }

        [Fact]
        public void Secao_Deve_Retornar_ConfiguracaoRabbitLog()
        {
            Assert.Equal("ConfiguracaoRabbitLog", RabbitLogOptions.Secao);
        }
    }
}

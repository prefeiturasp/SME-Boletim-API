using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class RabbitOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var hostName = "localhost";
            var userName = "guest";
            var password = "guest";
            var virtualHost = "/";
            ushort limite = 100;

            var options = new RabbitOptions
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                LimiteDeMensagensPorExecucao = limite
            };

            Assert.Equal(hostName, options.HostName);
            Assert.Equal(userName, options.UserName);
            Assert.Equal(password, options.Password);
            Assert.Equal(virtualHost, options.VirtualHost);
            Assert.Equal(limite, options.LimiteDeMensagensPorExecucao);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new RabbitOptions
            {
                HostName = null,
                UserName = null,
                Password = null,
                VirtualHost = null,
                LimiteDeMensagensPorExecucao = 0
            };

            Assert.Null(options.HostName);
            Assert.Null(options.UserName);
            Assert.Null(options.Password);
            Assert.Null(options.VirtualHost);
            Assert.Equal(0, options.LimiteDeMensagensPorExecucao);
        }
    }
}

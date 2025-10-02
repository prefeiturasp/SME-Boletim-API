using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class ConnectionStringOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var apiSerapExterna = "Server=localhost;Database=SerapExterna;";
            var apiSerap = "Server=localhost;Database=Serap;";
            var apiSerapLeitura = "Server=localhost;Database=SerapLeitura;";

            var options = new ConnectionStringOptions
            {
                ApiSerapExterna = apiSerapExterna,
                ApiSerap = apiSerap,
                ApiSerapLeitura = apiSerapLeitura
            };

            Assert.Equal(apiSerapExterna, options.ApiSerapExterna);
            Assert.Equal(apiSerap, options.ApiSerap);
            Assert.Equal(apiSerapLeitura, options.ApiSerapLeitura);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new ConnectionStringOptions
            {
                ApiSerapExterna = null,
                ApiSerap = null,
                ApiSerapLeitura = null
            };

            Assert.Null(options.ApiSerapExterna);
            Assert.Null(options.ApiSerap);
            Assert.Null(options.ApiSerapLeitura);
        }
    }
}

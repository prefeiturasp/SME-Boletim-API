using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class GithubOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var url = "https://github.com/org";
            var repositorioApi = "serap-api";
            var repositorioFront = "serap-front";

            var options = new GithubOptions
            {
                Url = url,
                RepositorioApi = repositorioApi,
                RepositorioFront = repositorioFront
            };

            Assert.Equal(url, options.Url);
            Assert.Equal(repositorioApi, options.RepositorioApi);
            Assert.Equal(repositorioFront, options.RepositorioFront);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var options = new GithubOptions
            {
                Url = null,
                RepositorioApi = null,
                RepositorioFront = null
            };

            Assert.Null(options.Url);
            Assert.Null(options.RepositorioApi);
            Assert.Null(options.RepositorioFront);
        }
    }
}

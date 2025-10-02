using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Teste.EnvironmentVariables
{
    public class TelemetriaOptionsTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new TelemetriaOptions
            {
                ApplicationInsights = true,
                Apm = false
            };

            Assert.True(options.ApplicationInsights);
            Assert.False(options.Apm);
        }

        [Fact]
        public void Propriedades_Por_Padrao_Sao_Falsas()
        {
            var options = new TelemetriaOptions();

            Assert.False(options.ApplicationInsights);
            Assert.False(options.Apm);
        }

        [Fact]
        public void Secao_Deve_Retornar_Telemetria()
        {
            Assert.Equal("Telemetria", TelemetriaOptions.Secao);
        }
    }
}

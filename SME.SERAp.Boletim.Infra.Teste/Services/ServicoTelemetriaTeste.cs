using Microsoft.ApplicationInsights;
using Moq;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.Infra.Teste.Services
{
    public class ServicoTelemetriaTeste
    {
        private TelemetriaOptions telemetriaOptions;
        private ServicoTelemetria servicoTelemetria;

        public ServicoTelemetriaTeste()
        {
        }

        private void InicializarServico(bool applicationInsights = false, bool apm = false)
        {
            telemetriaOptions = new TelemetriaOptions
            {
                ApplicationInsights = applicationInsights,
                Apm = apm
            };

            var cliente = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration());
            servicoTelemetria = new ServicoTelemetria(cliente, telemetriaOptions);
        }

        [Fact]
        public void Construtor_Com_TelemetriaOptions_Null_Deve_Lancar_Exception()
        {
            var cliente = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration());
            Assert.Throws<ArgumentNullException>(() => new ServicoTelemetria(cliente, null));
        }

        [Fact]
        public async Task RegistrarComRetornoAsync_Apm_False_Deve_Executar_Acao()
        {
            InicializarServico(applicationInsights: false, apm: false);

            var resultado = await servicoTelemetria.RegistrarComRetornoAsync<string>(() => Task.FromResult<object>("teste"), "acao", "telemetria", "valor");

            Assert.Equal("teste", resultado);
        }

        [Fact]
        public void RegistrarComRetorno_Apm_False_Deve_Executar_Acao()
        {
            InicializarServico(applicationInsights: false, apm: false);

            var resultado = servicoTelemetria.RegistrarComRetorno<string>(() => "teste", "acao", "telemetria", "valor");

            Assert.Equal("teste", resultado);
        }

        [Fact]
        public void Registrar_Apm_False_Deve_Executar_Acao()
        {
            InicializarServico(applicationInsights: false, apm: false);

            bool executou = false;
            servicoTelemetria.Registrar(() => executou = true, "acao", "telemetria", "valor");

            Assert.True(executou);
        }

        [Fact]
        public async Task RegistrarAsync_Apm_False_Deve_Executar_Acao()
        {
            InicializarServico(applicationInsights: false, apm: false);

            bool executou = false;
            await servicoTelemetria.RegistrarAsync(async () => { await Task.Delay(1); executou = true; }, "acao", "telemetria", "valor");

            Assert.True(executou);
        }

        //[Fact]
        //public async Task RegistrarComRetornoAsync_Apm_True_Deve_Executar_Acao_E_Chamar_Apm()
        //{
        //    InicializarServico(applicationInsights: false, apm: true);

        //    bool acaoExecutada = false;
        //    var resultado = await servicoTelemetria.RegistrarComRetornoAsync<string>(async () => { acaoExecutada = true; await Task.Delay(1); return "resultado"; }, "acao", "telemetria", "valor");

        //    Assert.True(acaoExecutada);
        //    Assert.Equal("resultado", resultado);
        //}

        //[Fact]
        //public void RegistrarComRetorno_Apm_True_Deve_Executar_Acao()
        //{
        //    InicializarServico(applicationInsights: false, apm: true);

        //    bool executou = false;
        //    var resultado = servicoTelemetria.RegistrarComRetorno<string>(() => { executou = true; return "ok"; }, "acao", "telemetria", "valor");

        //    Assert.True(executou);
        //    Assert.Equal("ok", resultado);
        //}

        //[Fact]
        //public void Registrar_Apm_True_Deve_Executar_Acao()
        //{
        //    InicializarServico(applicationInsights: false, apm: true);

        //    bool executou = false;
        //    servicoTelemetria.Registrar(() => executou = true, "acao", "telemetria", "valor");

        //    Assert.True(executou);
        //}

        //[Fact]
        //public async Task RegistrarAsync_Apm_True_Deve_Executar_Acao()
        //{
        //    InicializarServico(applicationInsights: false, apm: true);

        //    bool executou = false;
        //    await servicoTelemetria.RegistrarAsync(async () => { await Task.Delay(1); executou = true; }, "acao", "telemetria", "valor");

        //    Assert.True(executou);
        //}
    }
}

using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Teste.Extensions
{
    public class BoletimExtensionsTeste
    {
        [Fact]
        public void ObterUeDescricao_Deve_Retornar_Descricao_Correta()
        {
            string ueNome = "Escola Modelo";
            TipoEscola tipoEscola = TipoEscola.EMEF;
            string dreNome = "DIRETORIA REGIONAL DE EDUCACAO XYZ";
            string dreAbreviacao = "DRE - XYZ";

            var resultado = ueNome.ObterUeDescricao(tipoEscola, dreNome, dreAbreviacao);

            Assert.Equal("DRE XYZ - EMEF Escola Modelo", resultado);
        }

        [Fact]
        public void ObterNomeDreAbreviado_Deve_Substituir_Texto()
        {
            string dreNome = "DIRETORIA REGIONAL DE EDUCACAO ABC";

            var resultado = dreNome.ObterNomeDreAbreviado();

            Assert.Equal("DRE ABC", resultado);
        }

        [Fact]
        public void CalcularPercentual_Deve_Calcular_Corretamente()
        {
            decimal valorInicial = 200m;
            decimal valorFinal = 250m;

            var resultado = valorFinal.CalcularPercentual(valorInicial);

            Assert.Equal(25.0, resultado);
        }

        [Fact]
        public void CalcularPercentual_Com_ValorInicial_Zero_Deve_Retornar_Zero()
        {
            decimal valorInicial = 0m;
            decimal valorFinal = 100m;

            var resultado = valorFinal.CalcularPercentual(valorInicial);

            Assert.Equal(0.0, resultado);
        }
    }
}

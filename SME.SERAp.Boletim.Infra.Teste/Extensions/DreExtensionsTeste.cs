using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Teste.Extensions
{
    public class DreExtensionsTeste
    {
        [Fact]
        public void AbreviarPrefixoDre_Deve_Substituir_Prefixo_Por_DRE()
        {
            string dreNome = "DIRETORIA REGIONAL DE EDUCACAO ABC";

            var resultado = dreNome.AbreviarPrefixoDre();

            Assert.Equal("DRE ABC", resultado);
        }

        [Fact]
        public void AbreviarPrefixoDre_Com_Null_Deve_Retornar_Null()
        {
            string dreNome = null;

            var resultado = dreNome.AbreviarPrefixoDre();

            Assert.Null(resultado);
        }

        [Fact]
        public void RemoverPrefixoDre_Deve_Remover_Prefixo()
        {
            string dreNome = "DIRETORIA REGIONAL DE EDUCACAO XYZ";

            var resultado = dreNome.RemoverPrefixoDre();

            Assert.Equal("XYZ", resultado);
        }

        [Fact]
        public void RemoverPrefixoDre_Com_Null_Deve_Retornar_Null()
        {
            string dreNome = null;

            var resultado = dreNome.RemoverPrefixoDre();

            Assert.Null(resultado);
        }
    }
}

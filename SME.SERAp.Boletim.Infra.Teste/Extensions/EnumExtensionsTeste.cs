using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Boletim.Infra.Teste.Extensions
{
    public class EnumExtensionsTeste
    {
        private enum TesteEnum
        {
            Valor1,
            Valor2
        }

        private class TesteAtributo : Attribute { }

        [Fact]
        public void GetAttribute_Deve_Cair_No_Catch_Quando_Enum_Invalido()
        {
            var valorInvalido = (TesteEnum)999;

            var resultado = valorInvalido.GetAttribute<TesteAtributo>();

            Assert.Null(resultado);
        }

        [Fact]
        public void GetAttribute_Deve_Retornar_Atributo_Quando_Presente()
        {
            var valor = TipoEscola.EMEF;

            var atributo = valor.GetAttribute<DisplayAttribute>();

            Assert.NotNull(atributo);
            Assert.Equal("EMEF", atributo.ShortName);
        }

        [Fact]
        public void GetAttribute_Deve_Retornar_NA_Quando_Atributo_For_Nenhum()
        {
            var valor = TipoEscola.Nenhum;

            var atributo = valor.GetAttribute<DisplayAttribute>();

            Assert.Equal("NA", atributo.ShortName);
        }

        [Fact]
        public void GetAttribute_Deve_Retornar_Null_Quando_Tipo_Diferente()
        {
            var valor = TipoEscola.EMEFM;

            var atributo = valor.GetAttribute<ObsoleteAttribute>();

            Assert.Null(atributo);
        }
    }
}

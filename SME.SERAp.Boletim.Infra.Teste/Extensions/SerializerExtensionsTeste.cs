using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Teste.Extensions
{
    public class SerializerExtensionsTeste
    {
        private class TesteDto
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
            public string Nulo { get; set; }
        }

        [Fact]
        public void ConverterObjectStringPraObjeto_Deve_Deserializar_Corretamente()
        {
            var json = "{\"Nome\":\"Joao\",\"Idade\":30}";

            var obj = json.ConverterObjectStringPraObjeto<TesteDto>();

            Assert.NotNull(obj);
            Assert.Equal("Joao", obj.Nome);
            Assert.Equal(30, obj.Idade);
            Assert.Null(obj.Nulo);
        }

        [Fact]
        public void ConverterObjectStringPraObjeto_Com_String_Nula_Ou_Vazia_Deve_Retornar_Default()
        {
            string jsonNull = null;
            string jsonEmpty = string.Empty;

            var objNull = jsonNull.ConverterObjectStringPraObjeto<TesteDto>();
            var objEmpty = jsonEmpty.ConverterObjectStringPraObjeto<TesteDto>();

            Assert.Null(objNull);
            Assert.Null(objEmpty);
        }

        [Fact]
        public void ConverterObjectParaJson_Deve_Serializar_Corretamente()
        {
            var obj = new TesteDto { Nome = "Maria", Idade = 25, Nulo = null };

            var json = obj.ConverterObjectParaJson();

            Assert.Contains("\"Nome\":\"Maria\"", json);
            Assert.Contains("\"Idade\":25", json);
            Assert.DoesNotContain("Nulo", json);
        }

        [Fact]
        public void ConverterObjectParaJson_Com_Null_Deve_Retornar_String_Vazia()
        {
            object obj = null;

            var json = obj.ConverterObjectParaJson();

            Assert.Equal(string.Empty, json);
        }
    }
}

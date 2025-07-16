using SME.SERAp.Boletim.Dominio.Enumerados;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class LogNivelTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_LogNivel()
        {
            Assert.Equal(1, (int)LogNivel.Informacao);
            Assert.Equal(2, (int)LogNivel.Critico);
            Assert.Equal(3, (int)LogNivel.Negocio);
        }

        [Fact]
        public void Deve_Converter_De_Int_Para_LogNivel_Corretamente()
        {
            var nivel = (LogNivel)2;
            Assert.Equal(LogNivel.Critico, nivel);
        }

        [Fact]
        public void Deve_Converter_De_LogNivel_Para_String()
        {
            var nivel = LogNivel.Negocio;
            Assert.Equal("Negocio", nivel.ToString());
        }
    }
}

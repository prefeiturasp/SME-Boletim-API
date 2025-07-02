using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class TipoNivelProficienciaTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_TipoNivelProficiencia()
        {
            Assert.Equal(1, (int)TipoNivelProficiencia.AbaixoBasico);
            Assert.Equal(2, (int)TipoNivelProficiencia.Basico);
            Assert.Equal(3, (int)TipoNivelProficiencia.Adequado);
            Assert.Equal(4, (int)TipoNivelProficiencia.Avancado);
        }

        [Theory]
        [InlineData(TipoNivelProficiencia.AbaixoBasico, "Abaixo do básico")]
        [InlineData(TipoNivelProficiencia.Basico, "Básico")]
        [InlineData(TipoNivelProficiencia.Adequado, "Adequado")]
        [InlineData(TipoNivelProficiencia.Avancado, "Avançado")]
        public void Deve_Conter_Display_Name_Correto(TipoNivelProficiencia nivel, string nomeEsperado)
        {
            var display = nivel
                .GetType()
                .GetMember(nivel.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(display);
            Assert.Equal(nomeEsperado, display.Name);
        }
    }
}

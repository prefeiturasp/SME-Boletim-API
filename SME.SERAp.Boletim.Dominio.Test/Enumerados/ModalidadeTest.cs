using SME.SERAp.Boletim.Dominio.Enumerados;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class ModalidadeTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_Modalidade()
        {
            Assert.Equal(0, (int)Modalidade.NaoCadastrado);
            Assert.Equal(1, (int)Modalidade.EducacaoInfantil);
            Assert.Equal(3, (int)Modalidade.EJA);
            Assert.Equal(4, (int)Modalidade.CIEJA);
            Assert.Equal(5, (int)Modalidade.Fundamental);
            Assert.Equal(6, (int)Modalidade.Medio);
            Assert.Equal(7, (int)Modalidade.CMCT);
            Assert.Equal(8, (int)Modalidade.MOVA);
            Assert.Equal(9, (int)Modalidade.ETEC);
        }

        [Theory]
        [InlineData(Modalidade.EducacaoInfantil, "Educação Infantil", "EI")]
        [InlineData(Modalidade.EJA, "Educação de Jovens e Adultos", "EJA")]
        [InlineData(Modalidade.CIEJA, "CIEJA", "CIEJA")]
        [InlineData(Modalidade.Fundamental, "Ensino Fundamental", "EF")]
        [InlineData(Modalidade.Medio, "Ensino Médio", "EM")]
        [InlineData(Modalidade.CMCT, "CMCT", "CMCT")]
        [InlineData(Modalidade.MOVA, "MOVA", "MOVA")]
        [InlineData(Modalidade.ETEC, "ETEC", "ETEC")]
        public void Deve_Conter_Display_Corretos(Modalidade modalidade, string nomeEsperado, string shortNameEsperado)
        {
            var display = modalidade
                .GetType()
                .GetMember(modalidade.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(display);
            Assert.Equal(nomeEsperado, display.Name);
            Assert.Equal(shortNameEsperado, display.ShortName);
        }
    }
}

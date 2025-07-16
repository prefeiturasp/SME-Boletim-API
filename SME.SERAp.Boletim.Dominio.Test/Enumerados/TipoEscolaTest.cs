using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class TipoEscolaTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_TipoEscola()
        {
            Assert.Equal(0, (int)TipoEscola.Nenhum);
            Assert.Equal(1, (int)TipoEscola.EMEF);
            Assert.Equal(2, (int)TipoEscola.EMEI);
            Assert.Equal(3, (int)TipoEscola.EMEFM);
            Assert.Equal(4, (int)TipoEscola.EMEBS);
            Assert.Equal(10, (int)TipoEscola.CEIDIRET);
            Assert.Equal(11, (int)TipoEscola.CEIINDIR);
            Assert.Equal(12, (int)TipoEscola.CRPCONV);
            Assert.Equal(13, (int)TipoEscola.CIEJA);
            Assert.Equal(14, (int)TipoEscola.CCICIPS);
            Assert.Equal(15, (int)TipoEscola.ESCPART);
            Assert.Equal(16, (int)TipoEscola.CEUEMEF);
            Assert.Equal(17, (int)TipoEscola.CEUEMEI);
            Assert.Equal(18, (int)TipoEscola.CEUCEI);
            Assert.Equal(19, (int)TipoEscola.CEU);
            Assert.Equal(22, (int)TipoEscola.MOVA);
            Assert.Equal(23, (int)TipoEscola.CMCT);
            Assert.Equal(25, (int)TipoEscola.ETEC);
            Assert.Equal(26, (int)TipoEscola.ESPCONV);
            Assert.Equal(27, (int)TipoEscola.CEUATCOMPL);
            Assert.Equal(29, (int)TipoEscola.CCA);
            Assert.Equal(28, (int)TipoEscola.CEMEI);
            Assert.Equal(30, (int)TipoEscola.CECI);
            Assert.Equal(31, (int)TipoEscola.CEUCEMEI);
            Assert.Equal(32, (int)TipoEscola.EMEFPFOM);
            Assert.Equal(33, (int)TipoEscola.EMEIPFOM);
        }

        [Theory]
        [InlineData(TipoEscola.Nenhum, "Não Informado", "NA")]
        [InlineData(TipoEscola.EMEF, "Escola Municipal de Ensino Fundamental", "EMEF")]
        [InlineData(TipoEscola.EMEI, "Escola Municipal de Educação Infantil", "EMEI")]
        [InlineData(TipoEscola.EMEFM, "Escola Municipal de Ensino Fundamental e Médio", "EMEFM")]
        [InlineData(TipoEscola.EMEBS, "Escola Municipal de Ensino Bilíngue para Surdos", "EMEBS")]
        [InlineData(TipoEscola.CEIDIRET, "Centro de Educação Infantil Direto ", "CEI DIRET")]
        [InlineData(TipoEscola.CEIINDIR, "Centro de Educação Infantil Indireto", "CEI INDIR")]
        [InlineData(TipoEscola.CRPCONV, "Creche Particular COnveniada", "CR.P.CONV")]
        [InlineData(TipoEscola.CIEJA, "CENTRO INTEGRADO DE EDUCACAO DE JOVENS E ADULTOS", "CIEJA")]
        [InlineData(TipoEscola.CCICIPS, "CENTRO DE CONVIVENCIA INFANTIL /CENTRO INFANTIL DE PROTECAO A SAUDE", "CCI/CIPS")]
        [InlineData(TipoEscola.ESCPART, "ESCOLA PARTICULAR", "ESC.PART.")]
        [InlineData(TipoEscola.CEUEMEF, "Centro Unificado de Educação - Escola Municipal de Ensino Fundamental", "CEU EMEF")]
        [InlineData(TipoEscola.CEUEMEI, "Centro Unificado de Educação - Escola Municipal de Educação Infantil", "CEU EMEI")]
        [InlineData(TipoEscola.CEUCEI, "CENTRO EDUCACIONAL UNIFICADO - CEI", "CEU CEI")]
        [InlineData(TipoEscola.CEU, "CENTRO EDUCACIONAL UNIFICADO", "CEU")]
        [InlineData(TipoEscola.MOVA, "MOVIMENTO DE ALFABETIZACAO", "MOVA")]
        [InlineData(TipoEscola.CMCT, "CENTRO MUNICIPAL DE CAPACITACAO E TREIN.", "CMCT")]
        [InlineData(TipoEscola.ETEC, "ESCOLA TECNICA", "E TEC")]
        [InlineData(TipoEscola.ESPCONV, "ESPECIAL CONVENIADA", "ESP CONV")]
        [InlineData(TipoEscola.CEUATCOMPL, "CEU EXCLUSIVO ATIVIDADE COMPLEMENTAR", "CEU AT COMPL")]
        [InlineData(TipoEscola.CCA, "CENTRO PARA CRIANCAS E ADOLESCENTES", "CCA")]
        [InlineData(TipoEscola.CEMEI, "Centro Municipal de Educação Infantil", "CEMEI")]
        [InlineData(TipoEscola.CECI, "Centro de Educação e Cultura Indígena", "CECI")]
        [InlineData(TipoEscola.CEUCEMEI, "Centro Unificado de Educação - Centro Municipal de Educação Infantil", "CEU CEMEI")]
        [InlineData(TipoEscola.EMEFPFOM, "ESCOLA MUNICIPAL DE ENSINO FUNDAMENTAL PRIVADA FOMENTO", "EMEF P FOM")]
        [InlineData(TipoEscola.EMEIPFOM, "ESCOLA MUNICIPAL DE EDUCACAO INFANTIL PRIVADA FOMENTO", "EMEI P FOM")]
        public void Deve_Conter_Display_Name_E_ShortName_Corretos(TipoEscola tipoEscola, string nomeEsperado, string shortNameEsperado)
        {
            var display = tipoEscola
                .GetType()
                .GetMember(tipoEscola.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(display);
            Assert.Equal(nomeEsperado, display.Name);
            Assert.Equal(shortNameEsperado, display.ShortName);
        }
    }
}

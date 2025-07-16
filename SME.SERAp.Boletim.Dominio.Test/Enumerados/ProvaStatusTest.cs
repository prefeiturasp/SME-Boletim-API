using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class ProvaStatusTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_ProvaStatus()
        {
            Assert.Equal(0, (int)ProvaStatus.NaoIniciado);
            Assert.Equal(1, (int)ProvaStatus.Iniciado);
            Assert.Equal(2, (int)ProvaStatus.Finalizado);
            Assert.Equal(3, (int)ProvaStatus.Pendente);
            Assert.Equal(4, (int)ProvaStatus.EmRevisao);
            Assert.Equal(5, (int)ProvaStatus.FINALIZADA_AUTOMATICAMENTE_JOB);
            Assert.Equal(6, (int)ProvaStatus.FINALIZADA_AUTOMATICAMENTE_TEMPO);
            Assert.Equal(7, (int)ProvaStatus.FINALIZADA_OFFLINE);
        }

        [Theory]
        [InlineData(ProvaStatus.NaoIniciado, "Não Iniciado")]
        [InlineData(ProvaStatus.Iniciado, "Iniciado")]
        [InlineData(ProvaStatus.Finalizado, "Finalizado")]
        [InlineData(ProvaStatus.Pendente, "Pendente")]
        [InlineData(ProvaStatus.EmRevisao, "Em Revisão")]
        [InlineData(ProvaStatus.FINALIZADA_AUTOMATICAMENTE_JOB, "Finalizado Automaticamente")]
        [InlineData(ProvaStatus.FINALIZADA_AUTOMATICAMENTE_TEMPO, "Finalizado Automaticamente por Tempo")]
        [InlineData(ProvaStatus.FINALIZADA_OFFLINE, "Finalizado Offile")]
        public void Deve_Conter_Display_Name_Corretos(ProvaStatus status, string nomeEsperado)
        {
            var display = status
                .GetType()
                .GetMember(status.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(display);
            Assert.Equal(nomeEsperado, display.Name);
        }
    }
}

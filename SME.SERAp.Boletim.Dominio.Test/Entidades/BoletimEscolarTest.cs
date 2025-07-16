using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class BoletimEscolarTest
    {
        [Fact]
        public void Deve_Criar_BoletimEscolar_Com_Propriedades_Validas()
        {
            var boletim = new BoletimEscolar
            {
                UeId = 1,
                ProvaId = 2,
                ComponenteCurricular = "Matemática",
                AbaixoBasico = 5,
                AbaixoBasicoPorcentagem = 10,
                Basico = 15,
                BasicoPorcentagem = 30,
                Adequado = 20,
                AdequadoPorcentagem = 40,
                Avancado = 10,
                AvancadoPorcentagem = 20,
                Total = 50,
                MediaProficiencia = 250.5m
            };

            Assert.Equal(1, boletim.UeId);
            Assert.Equal(2, boletim.ProvaId);
            Assert.Equal("Matemática", boletim.ComponenteCurricular);
            Assert.Equal(5, boletim.AbaixoBasico);
            Assert.Equal(10, boletim.AbaixoBasicoPorcentagem);
            Assert.Equal(15, boletim.Basico);
            Assert.Equal(30, boletim.BasicoPorcentagem);
            Assert.Equal(20, boletim.Adequado);
            Assert.Equal(40, boletim.AdequadoPorcentagem);
            Assert.Equal(10, boletim.Avancado);
            Assert.Equal(20, boletim.AvancadoPorcentagem);
            Assert.Equal(50, boletim.Total);
            Assert.Equal(250.5m, boletim.MediaProficiencia);
        }
    }
}

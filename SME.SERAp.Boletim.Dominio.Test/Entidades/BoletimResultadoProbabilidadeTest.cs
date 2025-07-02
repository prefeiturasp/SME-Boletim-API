using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class BoletimResultadoProbabilidadeTest
    {
        [Fact]
        public void Deve_Criar_BoletimResultadoProbabilidade_Com_Propriedades_Validas()
        {
            var boletim = new BoletimResultadoProbabilidade
            {
                HabilidadeId = 1,
                CodigoHabilidade = "EF05MA01",
                HabilidadeDescricao = "Resolver problemas de adição",
                TurmaDescricao = "5º Ano A",
                TurmaId = 10,
                ProvaId = 20,
                UeId = 30,
                DisciplinaId = 40,
                AnoEscolar = 5,
                AbaixoDoBasico = 15.5m,
                Basico = 25.0m,
                Adequado = 35.0m,
                Avancado = 24.5m,
                DataConsolidacao = new DateTime(2025, 5, 1)
            };

            Assert.Equal(1, boletim.HabilidadeId);
            Assert.Equal("EF05MA01", boletim.CodigoHabilidade);
            Assert.Equal("Resolver problemas de adição", boletim.HabilidadeDescricao);
            Assert.Equal("5º Ano A", boletim.TurmaDescricao);
            Assert.Equal(10, boletim.TurmaId);
            Assert.Equal(20, boletim.ProvaId);
            Assert.Equal(30, boletim.UeId);
            Assert.Equal(40, boletim.DisciplinaId);
            Assert.Equal(5, boletim.AnoEscolar);
            Assert.Equal(15.5m, boletim.AbaixoDoBasico);
            Assert.Equal(25.0m, boletim.Basico);
            Assert.Equal(35.0m, boletim.Adequado);
            Assert.Equal(24.5m, boletim.Avancado);
            Assert.Equal(new DateTime(2025, 5, 1), boletim.DataConsolidacao);
        }
    }
}

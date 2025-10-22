using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciasGraficoComparativoDreDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Descricao_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Descricao = "Matemática 5º Ano"
            };

            Assert.Equal("Matemática 5º Ano", dto.Descricao);
        }

        [Fact]
        public void Deve_Atribuir_Mes_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Mes = "Janeiro"
            };

            Assert.Equal("Janeiro", dto.Mes);
        }

        [Fact]
        public void Deve_Atribuir_ValorProficiencia_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                ValorProficiencia = 8.5m
            };

            Assert.Equal(8.5m, dto.ValorProficiencia);
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Descricao = "Português 3º Ano",
                Mes = "Março",
                ValorProficiencia = 7.8m
            };

            Assert.Equal("Português 3º Ano", dto.Descricao);
            Assert.Equal("Março", dto.Mes);
            Assert.Equal(7.8m, dto.ValorProficiencia);
        }

        [Fact]
        public void Deve_Permitir_ValorProficiencia_Zero()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                ValorProficiencia = 0m
            };

            Assert.Equal(0m, dto.ValorProficiencia);
        }

        [Fact]
        public void Deve_Permitir_ValorProficiencia_Negativo()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                ValorProficiencia = -5.5m
            };

            Assert.Equal(-5.5m, dto.ValorProficiencia);
        }

        [Fact]
        public void Deve_Permitir_ValorProficiencia_Decimal_Com_Multiplas_Casas()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                ValorProficiencia = 9.876543m
            };

            Assert.Equal(9.876543m, dto.ValorProficiencia);
        }

        [Fact]
        public void Deve_Permitir_Descricao_Nula()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Descricao = null
            };

            Assert.Null(dto.Descricao);
        }

        [Fact]
        public void Deve_Permitir_Mes_Nulo()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Mes = null
            };

            Assert.Null(dto.Mes);
        }

        [Fact]
        public void Deve_Permitir_Descricao_Vazia()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Descricao = string.Empty
            };

            Assert.Equal(string.Empty, dto.Descricao);
        }

        [Fact]
        public void Deve_Permitir_Mes_Vazio()
        {
            var dto = new ProficienciasGraficoComparativoDreDto
            {
                Mes = string.Empty
            };

            Assert.Equal(string.Empty, dto.Mes);
        }
    }
}

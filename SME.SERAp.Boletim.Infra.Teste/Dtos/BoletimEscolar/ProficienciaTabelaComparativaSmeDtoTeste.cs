using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaTabelaComparativaSmeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var descricao = "SME Central";
            var mes = "Agosto";
            var valorProficiencia = 780.25m;
            var nivelProficiencia = "Avançado";
            var qtdeUe = 120;
            var qtdeDre = 13;
            var qtdeEstudante = 25000;

            var dto = new ProficienciaTabelaComparativaSmeDto
            {
                Descricao = descricao,
                Mes = mes,
                ValorProficiencia = valorProficiencia,
                NivelProficiencia = nivelProficiencia,
                QtdeUe = qtdeUe,
                QtdeDre = qtdeDre,
                QtdeEstudante = qtdeEstudante
            };

            Assert.Equal(descricao, dto.Descricao);
            Assert.Equal(mes, dto.Mes);
            Assert.Equal(valorProficiencia, dto.ValorProficiencia);
            Assert.Equal(nivelProficiencia, dto.NivelProficiencia);
            Assert.Equal(qtdeUe, dto.QtdeUe);
            Assert.Equal(qtdeDre, dto.QtdeDre);
            Assert.Equal(qtdeEstudante, dto.QtdeEstudante);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_String_Nulas()
        {
            var dto = new ProficienciaTabelaComparativaSmeDto
            {
                Descricao = null,
                Mes = null,
                NivelProficiencia = null,
                ValorProficiencia = 0m,
                QtdeUe = 0,
                QtdeDre = 0,
                QtdeEstudante = 0
            };

            Assert.Null(dto.Descricao);
            Assert.Null(dto.Mes);
            Assert.Null(dto.NivelProficiencia);
            Assert.Equal(0m, dto.ValorProficiencia);
            Assert.Equal(0, dto.QtdeUe);
            Assert.Equal(0, dto.QtdeDre);
            Assert.Equal(0, dto.QtdeEstudante);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var dto = new ProficienciaTabelaComparativaSmeDto
            {
                Descricao = string.Empty,
                Mes = "Dezembro",
                ValorProficiencia = decimal.MaxValue,
                NivelProficiencia = "Básico",
                QtdeUe = int.MaxValue,
                QtdeDre = int.MaxValue,
                QtdeEstudante = int.MaxValue
            };

            Assert.Equal(string.Empty, dto.Descricao);
            Assert.Equal("Dezembro", dto.Mes);
            Assert.Equal(decimal.MaxValue, dto.ValorProficiencia);
            Assert.Equal("Básico", dto.NivelProficiencia);
            Assert.Equal(int.MaxValue, dto.QtdeUe);
            Assert.Equal(int.MaxValue, dto.QtdeDre);
            Assert.Equal(int.MaxValue, dto.QtdeEstudante);
        }
    }
}

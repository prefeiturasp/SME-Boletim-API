using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaTabelaComparativaDreTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var descricao = "DRE Centro";
            var mes = "Setembro";
            var valorProficiencia = 650.75m;
            var nivelProficiencia = "Avançado";
            var qtdeUe = 15;
            var qtdeEstudante = 1200;

            var dto = new ProficienciaTabelaComparativaDre
            {
                Descricao = descricao,
                Mes = mes,
                ValorProficiencia = valorProficiencia,
                NivelProficiencia = nivelProficiencia,
                QtdeUe = qtdeUe,
                QtdeEstudante = qtdeEstudante
            };

            Assert.Equal(descricao, dto.Descricao);
            Assert.Equal(mes, dto.Mes);
            Assert.Equal(valorProficiencia, dto.ValorProficiencia);
            Assert.Equal(nivelProficiencia, dto.NivelProficiencia);
            Assert.Equal(qtdeUe, dto.QtdeUe);
            Assert.Equal(qtdeEstudante, dto.QtdeEstudante);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_String_Nulas()
        {
            var dto = new ProficienciaTabelaComparativaDre
            {
                Descricao = null,
                Mes = null,
                NivelProficiencia = null,
                ValorProficiencia = 0m,
                QtdeUe = 0,
                QtdeEstudante = 0
            };

            Assert.Null(dto.Descricao);
            Assert.Null(dto.Mes);
            Assert.Null(dto.NivelProficiencia);
            Assert.Equal(0m, dto.ValorProficiencia);
            Assert.Equal(0, dto.QtdeUe);
            Assert.Equal(0, dto.QtdeEstudante);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var dto = new ProficienciaTabelaComparativaDre
            {
                Descricao = string.Empty,
                Mes = "Dezembro",
                ValorProficiencia = decimal.MaxValue,
                NivelProficiencia = "Básico",
                QtdeUe = int.MaxValue,
                QtdeEstudante = int.MaxValue
            };

            Assert.Equal(string.Empty, dto.Descricao);
            Assert.Equal("Dezembro", dto.Mes);
            Assert.Equal(decimal.MaxValue, dto.ValorProficiencia);
            Assert.Equal("Básico", dto.NivelProficiencia);
            Assert.Equal(int.MaxValue, dto.QtdeUe);
            Assert.Equal(int.MaxValue, dto.QtdeEstudante);
        }
    }
}

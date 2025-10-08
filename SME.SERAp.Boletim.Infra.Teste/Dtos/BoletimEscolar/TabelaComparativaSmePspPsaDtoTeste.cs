using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class TabelaComparativaSmePspPsaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var variacao = 12.5m;
            var aplicacao = new List<ProficienciaTabelaComparativaSmeDto>
            {
                new ProficienciaTabelaComparativaSmeDto
                {
                    Descricao = "SME Norte",
                    Mes = "Março",
                    ValorProficiencia = 700.10m,
                    NivelProficiencia = "Intermediário",
                    QtdeUe = 90,
                    QtdeDre = 10,
                    QtdeEstudante = 18000
                }
            };

            var dto = new TabelaComparativaSmePspPsaDto
            {
                Variacao = variacao,
                Aplicacao = aplicacao
            };

            Assert.Equal(variacao, dto.Variacao);
            Assert.Equal(aplicacao, dto.Aplicacao);
            Assert.Single(dto.Aplicacao);
            Assert.Equal("SME Norte", dto.Aplicacao.First().Descricao);
        }

        [Fact]
        public void Deve_Permitir_Aplicacao_Nula()
        {
            var dto = new TabelaComparativaSmePspPsaDto
            {
                Variacao = 0m,
                Aplicacao = null
            };

            Assert.Equal(0m, dto.Variacao);
            Assert.Null(dto.Aplicacao);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var aplicacao = new List<ProficienciaTabelaComparativaSmeDto>
            {
                new ProficienciaTabelaComparativaSmeDto
                {
                    Descricao = string.Empty,
                    Mes = "Dezembro",
                    ValorProficiencia = decimal.MaxValue,
                    NivelProficiencia = "Avançado",
                    QtdeUe = int.MaxValue,
                    QtdeDre = int.MaxValue,
                    QtdeEstudante = int.MaxValue
                }
            };

            var dto = new TabelaComparativaSmePspPsaDto
            {
                Variacao = decimal.MaxValue,
                Aplicacao = aplicacao
            };

            Assert.Equal(decimal.MaxValue, dto.Variacao);
            Assert.NotNull(dto.Aplicacao);
            Assert.Single(dto.Aplicacao);
            var item = dto.Aplicacao.First();
            Assert.Equal(decimal.MaxValue, item.ValorProficiencia);
            Assert.Equal(int.MaxValue, item.QtdeUe);
            Assert.Equal(int.MaxValue, item.QtdeDre);
            Assert.Equal(int.MaxValue, item.QtdeEstudante);
        }
    }
}

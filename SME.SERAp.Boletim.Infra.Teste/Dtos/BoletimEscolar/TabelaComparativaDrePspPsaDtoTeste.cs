using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class TabelaComparativaDrePspPsaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var variacao = 15.75m;
            var aplicacoes = new List<ProficienciaTabelaComparativaDre>
            {
                new ProficienciaTabelaComparativaDre
                {
                    Descricao = "DRE Centro",
                    Mes = "Setembro",
                    ValorProficiencia = 700m,
                    NivelProficiencia = "Avançado",
                    QtdeUe = 10,
                    QtdeEstudante = 500
                },
                new ProficienciaTabelaComparativaDre
                {
                    Descricao = "DRE Norte",
                    Mes = "Setembro",
                    ValorProficiencia = 650m,
                    NivelProficiencia = "Intermediário",
                    QtdeUe = 8,
                    QtdeEstudante = 400
                }
            };

            var dto = new TabelaComparativaDrePspPsaDto
            {
                Variacao = variacao,
                Aplicacao = aplicacoes
            };

            Assert.Equal(variacao, dto.Variacao);
            Assert.Equal(aplicacoes, dto.Aplicacao);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var dto = new TabelaComparativaDrePspPsaDto
            {
                Variacao = 0m,
                Aplicacao = null
            };

            Assert.Equal(0m, dto.Variacao);
            Assert.Null(dto.Aplicacao);
        }

        [Fact]
        public void Deve_Atribuir_Lista_Vazia()
        {
            var dto = new TabelaComparativaDrePspPsaDto
            {
                Variacao = 0m,
                Aplicacao = new List<ProficienciaTabelaComparativaDre>()
            };

            Assert.Equal(0m, dto.Variacao);
            Assert.NotNull(dto.Aplicacao);
            Assert.Empty(dto.Aplicacao);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var dto = new TabelaComparativaDrePspPsaDto
            {
                Variacao = decimal.MaxValue,
                Aplicacao = new List<ProficienciaTabelaComparativaDre>
                {
                    new ProficienciaTabelaComparativaDre
                    {
                        Descricao = string.Empty,
                        Mes = "Dezembro",
                        ValorProficiencia = decimal.MaxValue,
                        NivelProficiencia = "Básico",
                        QtdeUe = int.MaxValue,
                        QtdeEstudante = int.MaxValue
                    }
                }
            };

            Assert.Equal(decimal.MaxValue, dto.Variacao);
            Assert.NotNull(dto.Aplicacao);
            Assert.Single(dto.Aplicacao);
            Assert.Equal(decimal.MaxValue, dto.Aplicacao.First().ValorProficiencia);
            Assert.Equal(int.MaxValue, dto.Aplicacao.First().QtdeUe);
            Assert.Equal(int.MaxValue, dto.Aplicacao.First().QtdeEstudante);
        }
    }
}

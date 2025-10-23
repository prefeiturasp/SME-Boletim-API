using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class CardComparativoProficienciaDreDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_DreId_Corretamente()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreId = 123456789L
            };

            Assert.Equal(123456789L, dto.DreId);
        }

        [Fact]
        public void Deve_Atribuir_DreAbreviacao_Corretamente()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreAbreviacao = "DRE-CS"
            };

            Assert.Equal("DRE-CS", dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Atribuir_DreNome_Corretamente()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreNome = "Diretoria Regional de Educação Campo Limpo"
            };

            Assert.Equal("Diretoria Regional de Educação Campo Limpo", dto.DreNome);
        }

        [Fact]
        public void Deve_Atribuir_Variacao_Corretamente()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                Variacao = 15.5m
            };

            Assert.Equal(15.5m, dto.Variacao);
        }

        [Fact]
        public void Deve_Atribuir_AplicacaoPsp_Corretamente()
        {
            var aplicacaoPsp = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = "PSP - Matemática",
                MediaProficiencia = 7.5m
            };

            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacaoPsp = aplicacaoPsp
            };

            Assert.Equal(aplicacaoPsp, dto.AplicacaoPsp);
            Assert.Equal("PSP - Matemática", dto.AplicacaoPsp.NomeAplicacao);
        }

        [Fact]
        public void Deve_Atribuir_AplicacoesPsa_Corretamente()
        {
            var aplicacoesPsa = new List<ProficienciaDetalheDreDto>
            {
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA 1" },
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA 2" },
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA 3" }
            };

            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacoesPsa = aplicacoesPsa
            };

            Assert.Equal(aplicacoesPsa, dto.AplicacoesPsa);
            Assert.Equal(3, dto.AplicacoesPsa.Count());
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var aplicacaoPsp = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = "PSP - Português"
            };

            var aplicacoesPsa = new List<ProficienciaDetalheDreDto>
            {
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA 1" },
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA 2" }
            };

            var dto = new CardComparativoProficienciaDreDto
            {
                DreId = 987654321L,
                DreAbreviacao = "DRE-BT",
                DreNome = "Diretoria Regional de Educação Butantã",
                Variacao = 8.3m,
                AplicacaoPsp = aplicacaoPsp,
                AplicacoesPsa = aplicacoesPsa
            };

            Assert.Equal(987654321L, dto.DreId);
            Assert.Equal("DRE-BT", dto.DreAbreviacao);
            Assert.Equal("Diretoria Regional de Educação Butantã", dto.DreNome);
            Assert.Equal(8.3m, dto.Variacao);
            Assert.Equal(aplicacaoPsp, dto.AplicacaoPsp);
            Assert.Equal(aplicacoesPsa, dto.AplicacoesPsa);
            Assert.Equal(2, dto.AplicacoesPsa.Count());
        }

        [Fact]
        public void Deve_Permitir_DreId_Zero()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreId = 0L
            };

            Assert.Equal(0L, dto.DreId);
        }

        [Fact]
        public void Deve_Permitir_DreId_Negativo()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreId = -123L
            };

            Assert.Equal(-123L, dto.DreId);
        }

        [Fact]
        public void Deve_Permitir_Variacao_Zero()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                Variacao = 0m
            };

            Assert.Equal(0m, dto.Variacao);
        }

        [Fact]
        public void Deve_Permitir_Variacao_Negativa()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                Variacao = -10.5m
            };

            Assert.Equal(-10.5m, dto.Variacao);
        }

        [Fact]
        public void Deve_Permitir_Variacao_Decimal_Com_Multiplas_Casas()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                Variacao = 12.345678m
            };

            Assert.Equal(12.345678m, dto.Variacao);
        }

        [Fact]
        public void Deve_Permitir_DreAbreviacao_Nula()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreAbreviacao = null
            };

            Assert.Null(dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Permitir_DreNome_Nulo()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreNome = null
            };

            Assert.Null(dto.DreNome);
        }

        [Fact]
        public void Deve_Permitir_DreAbreviacao_Vazia()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreAbreviacao = string.Empty
            };

            Assert.Equal(string.Empty, dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Permitir_DreNome_Vazio()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                DreNome = string.Empty
            };

            Assert.Equal(string.Empty, dto.DreNome);
        }

        [Fact]
        public void Deve_Permitir_AplicacaoPsp_Nulo()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacaoPsp = null
            };

            Assert.Null(dto.AplicacaoPsp);
        }

        [Fact]
        public void Deve_Permitir_AplicacoesPsa_Nulo()
        {
            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacoesPsa = null
            };

            Assert.Null(dto.AplicacoesPsa);
        }

        [Fact]
        public void Deve_Permitir_AplicacoesPsa_Vazio()
        {
            var aplicacoesPsa = new List<ProficienciaDetalheDreDto>();

            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacoesPsa = aplicacoesPsa
            };

            Assert.NotNull(dto.AplicacoesPsa);
            Assert.Empty(dto.AplicacoesPsa);
        }

        [Fact]
        public void Deve_Permitir_AplicacoesPsa_Com_Um_Item()
        {
            var aplicacoesPsa = new List<ProficienciaDetalheDreDto>
            {
                new ProficienciaDetalheDreDto { NomeAplicacao = "PSA Único" }
            };

            var dto = new CardComparativoProficienciaDreDto
            {
                AplicacoesPsa = aplicacoesPsa
            };

            Assert.Single(dto.AplicacoesPsa);
        }
    }
}

using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos
{
    public class ProficienciaDetalheDreDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_NomeAplicacao_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = "Prova de Matemática"
            };

            Assert.Equal("Prova de Matemática", dto.NomeAplicacao);
        }

        [Fact]
        public void Deve_Atribuir_Periodo_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                Periodo = "2024/1º Semestre"
            };

            Assert.Equal("2024/1º Semestre", dto.Periodo);
        }

        [Fact]
        public void Deve_Atribuir_MediaProficiencia_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                MediaProficiencia = 7.5m
            };

            Assert.Equal(7.5m, dto.MediaProficiencia);
        }

        [Fact]
        public void Deve_Atribuir_RealizaramProva_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                RealizaramProva = 150
            };

            Assert.Equal(150, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Atribuir_QuantidadeUes_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                QuantidadeUes = 25
            };

            Assert.Equal(25, dto.QuantidadeUes);
        }

        [Fact]
        public void Deve_Atribuir_NivelProficiencia_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                NivelProficiencia = "Avançado"
            };

            Assert.Equal("Avançado", dto.NivelProficiencia);
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = "Prova de Português",
                Periodo = "2024/2º Semestre",
                MediaProficiencia = 8.2m,
                RealizaramProva = 200,
                QuantidadeUes = 30,
                NivelProficiencia = "Proficiente"
            };

            Assert.Equal("Prova de Português", dto.NomeAplicacao);
            Assert.Equal("2024/2º Semestre", dto.Periodo);
            Assert.Equal(8.2m, dto.MediaProficiencia);
            Assert.Equal(200, dto.RealizaramProva);
            Assert.Equal(30, dto.QuantidadeUes);
            Assert.Equal("Proficiente", dto.NivelProficiencia);
        }

        [Fact]
        public void Deve_Permitir_MediaProficiencia_Zero()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                MediaProficiencia = 0m
            };

            Assert.Equal(0m, dto.MediaProficiencia);
        }

        [Fact]
        public void Deve_Permitir_RealizaramProva_Zero()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                RealizaramProva = 0
            };

            Assert.Equal(0, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Permitir_QuantidadeUes_Zero()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                QuantidadeUes = 0
            };

            Assert.Equal(0, dto.QuantidadeUes);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_String_Nulas()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = null,
                Periodo = null,
                NivelProficiencia = null
            };

            Assert.Null(dto.NomeAplicacao);
            Assert.Null(dto.Periodo);
            Assert.Null(dto.NivelProficiencia);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_String_Vazias()
        {
            var dto = new ProficienciaDetalheDreDto
            {
                NomeAplicacao = string.Empty,
                Periodo = string.Empty,
                NivelProficiencia = string.Empty
            };

            Assert.Equal(string.Empty, dto.NomeAplicacao);
            Assert.Equal(string.Empty, dto.Periodo);
            Assert.Equal(string.Empty, dto.NivelProficiencia);
        }
    }
}

using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ResultadoProeficienciaSmeTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 2001L;
            var nomeAplicacao = "Prova Brasil";
            var periodo = "2025-2";
            var quantidadeDres = 13;
            var quantidadeUes = 450;
            var anoEscolar = "5º Ano";
            var disciplinaId = 2;
            var mediaProficiencia = 650.75m;
            var realizaramProva = 9800;

            var dto = new ResultadoProeficienciaSme
            {
                LoteId = loteId,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                QuantidadeDres = quantidadeDres,
                QuantidadeUes = quantidadeUes,
                AnoEscolar = anoEscolar,
                DisciplinaId = disciplinaId,
                MediaProficiencia = mediaProficiencia,
                RealizaramProva = realizaramProva
            };

            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(quantidadeDres, dto.QuantidadeDres);
            Assert.Equal(quantidadeUes, dto.QuantidadeUes);
            Assert.Equal(anoEscolar, dto.AnoEscolar);
            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(realizaramProva, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Permitir_Strings_Nulas()
        {
            var dto = new ResultadoProeficienciaSme
            {
                LoteId = 0,
                NomeAplicacao = null,
                Periodo = null,
                QuantidadeDres = 0,
                QuantidadeUes = 0,
                AnoEscolar = null,
                DisciplinaId = 0,
                MediaProficiencia = 0m,
                RealizaramProva = 0
            };

            Assert.Equal(0, dto.LoteId);
            Assert.Null(dto.NomeAplicacao);
            Assert.Null(dto.Periodo);
            Assert.Equal(0, dto.QuantidadeDres);
            Assert.Equal(0, dto.QuantidadeUes);
            Assert.Null(dto.AnoEscolar);
            Assert.Equal(0, dto.DisciplinaId);
            Assert.Equal(0m, dto.MediaProficiencia);
            Assert.Equal(0, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var dto = new ResultadoProeficienciaSme
            {
                LoteId = long.MaxValue,
                NomeAplicacao = string.Empty,
                Periodo = "Dezembro/2025",
                QuantidadeDres = int.MaxValue,
                QuantidadeUes = int.MaxValue,
                AnoEscolar = "Ensino Fundamental",
                DisciplinaId = int.MaxValue,
                MediaProficiencia = decimal.MaxValue,
                RealizaramProva = int.MaxValue
            };

            Assert.Equal(long.MaxValue, dto.LoteId);
            Assert.Equal(string.Empty, dto.NomeAplicacao);
            Assert.Equal("Dezembro/2025", dto.Periodo);
            Assert.Equal(int.MaxValue, dto.QuantidadeDres);
            Assert.Equal(int.MaxValue, dto.QuantidadeUes);
            Assert.Equal("Ensino Fundamental", dto.AnoEscolar);
            Assert.Equal(int.MaxValue, dto.DisciplinaId);
            Assert.Equal(decimal.MaxValue, dto.MediaProficiencia);
            Assert.Equal(int.MaxValue, dto.RealizaramProva);
        }
    }
}

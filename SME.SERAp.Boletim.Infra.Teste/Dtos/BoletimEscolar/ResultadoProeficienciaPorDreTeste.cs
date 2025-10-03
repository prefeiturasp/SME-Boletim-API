using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ResultadoProeficienciaPorDreTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 1001L;
            var nomeAplicacao = "Prova SAEB";
            var periodo = "2025-1";
            var quantidadeUes = 25;
            var dreId = 7;
            var dreAbreviacao = "DRE-CEN";
            var dreNome = "Diretoria Regional Centro";
            var anoEscolar = "9º Ano";
            var disciplinaId = 3;
            var mediaProficiencia = 725.50m;
            var realizaramProva = 1200;

            var dto = new ResultadoProeficienciaPorDre
            {
                LoteId = loteId,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                QuantidadeUes = quantidadeUes,
                DreId = dreId,
                DreAbreviacao = dreAbreviacao,
                DreNome = dreNome,
                AnoEscolar = anoEscolar,
                DisciplinaId = disciplinaId,
                MediaProficiencia = mediaProficiencia,
                RealizaramProva = realizaramProva
            };

            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(quantidadeUes, dto.QuantidadeUes);
            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(dreAbreviacao, dto.DreAbreviacao);
            Assert.Equal(dreNome, dto.DreNome);
            Assert.Equal(anoEscolar, dto.AnoEscolar);
            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(realizaramProva, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Permitir_Strings_Nulas()
        {
            var dto = new ResultadoProeficienciaPorDre
            {
                LoteId = 0,
                NomeAplicacao = null,
                Periodo = null,
                QuantidadeUes = 0,
                DreId = 0,
                DreAbreviacao = null,
                DreNome = null,
                AnoEscolar = null,
                DisciplinaId = 0,
                MediaProficiencia = 0m,
                RealizaramProva = 0
            };

            Assert.Equal(0, dto.LoteId);
            Assert.Null(dto.NomeAplicacao);
            Assert.Null(dto.Periodo);
            Assert.Equal(0, dto.QuantidadeUes);
            Assert.Equal(0, dto.DreId);
            Assert.Null(dto.DreAbreviacao);
            Assert.Null(dto.DreNome);
            Assert.Null(dto.AnoEscolar);
            Assert.Equal(0, dto.DisciplinaId);
            Assert.Equal(0m, dto.MediaProficiencia);
            Assert.Equal(0, dto.RealizaramProva);
        }

        [Fact]
        public void Deve_Atribuir_Valores_Extremos()
        {
            var dto = new ResultadoProeficienciaPorDre
            {
                LoteId = long.MaxValue,
                NomeAplicacao = string.Empty,
                Periodo = "Dezembro/2025",
                QuantidadeUes = int.MaxValue,
                DreId = int.MaxValue,
                DreAbreviacao = "MAX",
                DreNome = "Nome Máximo",
                AnoEscolar = "Ensino Médio",
                DisciplinaId = int.MaxValue,
                MediaProficiencia = decimal.MaxValue,
                RealizaramProva = int.MaxValue
            };

            Assert.Equal(long.MaxValue, dto.LoteId);
            Assert.Equal(string.Empty, dto.NomeAplicacao);
            Assert.Equal("Dezembro/2025", dto.Periodo);
            Assert.Equal(int.MaxValue, dto.QuantidadeUes);
            Assert.Equal(int.MaxValue, dto.DreId);
            Assert.Equal("MAX", dto.DreAbreviacao);
            Assert.Equal("Nome Máximo", dto.DreNome);
            Assert.Equal("Ensino Médio", dto.AnoEscolar);
            Assert.Equal(int.MaxValue, dto.DisciplinaId);
            Assert.Equal(decimal.MaxValue, dto.MediaProficiencia);
            Assert.Equal(int.MaxValue, dto.RealizaramProva);
        }
    }
}

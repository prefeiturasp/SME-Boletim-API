using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class DreDisciplinaProficienciaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dreId = 1L;
            var dreNome = "DRE Teste";
            var anoEscolar = 5;
            var totalUes = 12;
            var totalAlunos = 300;
            var totalRealizaramProva = 280;
            var percentualParticipacao = 93.3m;
            var disciplina = "Português";
            var mediaProficiencia = 650.7m;
            var nivelProficiencia = "Adequado";

            var dto = new DreDisciplinaProficienciaDto
            {
                DreId = dreId,
                DreNome = dreNome,
                AnoEscolar = anoEscolar,
                TotalUes = totalUes,
                TotalAlunos = totalAlunos,
                TotalRealizaramProva = totalRealizaramProva,
                PercentualParticipacao = percentualParticipacao,
                Disciplina = disciplina,
                MediaProficiencia = mediaProficiencia,
                NivelProficiencia = nivelProficiencia
            };

            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(dreNome, dto.DreNome);
            Assert.Equal(anoEscolar, dto.AnoEscolar);
            Assert.Equal(totalUes, dto.TotalUes);
            Assert.Equal(totalAlunos, dto.TotalAlunos);
            Assert.Equal(totalRealizaramProva, dto.TotalRealizaramProva);
            Assert.Equal(percentualParticipacao, dto.PercentualParticipacao);
            Assert.Equal(disciplina, dto.Disciplina);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(nivelProficiencia, dto.NivelProficiencia);
        }

        [Fact]
        public void Deve_Permitir_Propriedades_Nulas()
        {
            var dto = new DreDisciplinaProficienciaDto
            {
                DreId = 2,
                DreNome = null,
                AnoEscolar = 9,
                TotalUes = 0,
                TotalAlunos = 0,
                TotalRealizaramProva = 0,
                PercentualParticipacao = 0m,
                Disciplina = null,
                MediaProficiencia = 0m,
                NivelProficiencia = null
            };

            Assert.Null(dto.DreNome);
            Assert.Null(dto.Disciplina);
            Assert.Null(dto.NivelProficiencia);
            Assert.Equal(0, dto.TotalUes);
            Assert.Equal(0, dto.TotalAlunos);
            Assert.Equal(0, dto.TotalRealizaramProva);
            Assert.Equal(0m, dto.PercentualParticipacao);
            Assert.Equal(0m, dto.MediaProficiencia);
        }
    }
}

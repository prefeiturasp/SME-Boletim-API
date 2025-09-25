using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class DisciplinaMediaProficienciaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var disciplinaId = 10;
            var disciplina = "Matemática";
            var mediaProficiencia = 725.5m;

            var dto = new DisciplinaMediaProficienciaDto
            {
                DisciplinaId = disciplinaId,
                Disciplina = disciplina,
                MediaProficiencia = mediaProficiencia
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(disciplina, dto.Disciplina);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
        }

        [Fact]
        public void Deve_Permitir_Disciplina_Nula()
        {
            var disciplinaId = 20;
            string disciplina = null;
            var mediaProficiencia = 500.0m;

            var dto = new DisciplinaMediaProficienciaDto
            {
                DisciplinaId = disciplinaId,
                Disciplina = disciplina,
                MediaProficiencia = mediaProficiencia
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Null(dto.Disciplina);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
        }

        [Fact]
        public void Deve_Atribuir_Valor_Zero_Para_MediaProficiencia()
        {
            var disciplinaId = 30;
            var disciplina = "História";
            var mediaProficiencia = 0m;

            var dto = new DisciplinaMediaProficienciaDto
            {
                DisciplinaId = disciplinaId,
                Disciplina = disciplina,
                MediaProficiencia = mediaProficiencia
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(disciplina, dto.Disciplina);
            Assert.Equal(0m, dto.MediaProficiencia);
        }
    }
}

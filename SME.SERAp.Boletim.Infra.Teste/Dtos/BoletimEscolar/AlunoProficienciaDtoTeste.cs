using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class AlunoProficienciaDtoTeste
    {
        [Fact(DisplayName = "DTO AlunoProficienciaDto deve ser criado com sucesso.")]
        public void AlunoProficienciaDto_DeveSerCriadoComSucesso()
        {
            var dto = new AlunoProficienciaDto();

            dto.AlunoRa = 12345;
            dto.NomeAluno = "Teste de Nome";
            dto.Proficiencia = 250.5M;
            dto.LoteId = 1;
            dto.Turma = "TURMA A";
            dto.NomeAplicacao = "PSA";
            dto.Periodo = "Agosto";
            dto.DisciplinaNome = "Matemática";
            dto.NomeLote = "Lote 2024";

            Assert.NotNull(dto);
            Assert.Equal(12345, dto.AlunoRa);
            Assert.Equal("Teste de Nome", dto.NomeAluno);
            Assert.Equal(250.5M, dto.Proficiencia);
            Assert.Equal(1, dto.LoteId);
            Assert.Equal("TURMA A", dto.Turma);
            Assert.Equal("PSA", dto.NomeAplicacao);
            Assert.Equal("Agosto", dto.Periodo);
            Assert.Equal("Matemática", dto.DisciplinaNome);
            Assert.Equal("Lote 2024", dto.NomeLote);
        }

        [Fact(DisplayName = "DTO AlunoProficienciaDto deve aceitar valores nulos em propriedades opcionais.")]
        public void AlunoProficienciaDto_DeveAceitarValoresNulos()
        {
            var dto = new AlunoProficienciaDto
            {
                AlunoRa = 99999,
                NomeAluno = "Aluno Sem Lote",
                Proficiencia = 300m,
                LoteId = 0
            };

            Assert.Null(dto.DisciplinaNome);
            Assert.Null(dto.NomeLote);
            Assert.Null(dto.NomeAplicacao);
            Assert.Null(dto.Periodo);
            Assert.Null(dto.Turma);
        }
    }
}

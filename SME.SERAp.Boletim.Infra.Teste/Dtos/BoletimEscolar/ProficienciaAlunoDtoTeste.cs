using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaAlunoDtoTeste
    {
        [Fact(DisplayName = "DTO ProficienciaAlunoDto deve ser criado com sucesso.")]
        public void ProficienciaAlunoDto_DeveSerCriadoComSucesso()
        {
            var dto = new ProficienciaAlunoDto();

            dto.Nome = "Aluno Teste";
            dto.Turma = "5A";
            dto.Variacao = 15.5;
            dto.Proficiencias = new List<ProficienciaDetalheDto>();

            Assert.NotNull(dto);
            Assert.Equal("Aluno Teste", dto.Nome);
            Assert.Equal("5A", dto.Turma);
            Assert.Equal(15.5, dto.Variacao);
            Assert.NotNull(dto.Proficiencias);
            Assert.Empty(dto.Proficiencias);
        }

        [Fact(DisplayName = "DTO ProficienciaAlunoDto deve conter proficiências preenchidas.")]
        public void ProficienciaAlunoDto_DeveConterProficienciasPreenchidas()
        {
            var dto = new ProficienciaAlunoDto
            {
                Nome = "Maria Santos",
                Turma = "6B",
                Variacao = -3.2,
                Proficiencias = new List<ProficienciaDetalheDto>
                {
                    new ProficienciaDetalheDto { Mes = string.Empty, Valor = 480m, NivelProficiencia = "Abaixo do Básico" },
                    new ProficienciaDetalheDto { Mes = "Abril", Valor = 510m, NivelProficiencia = "Básico" }
                }
            };

            Assert.Equal(2, dto.Proficiencias.Count());
            Assert.Equal(-3.2, dto.Variacao);
            Assert.Equal("6B", dto.Turma);
        }

        [Fact(DisplayName = "DTO ProficienciaAlunoDto deve aceitar variação zero.")]
        public void ProficienciaAlunoDto_DeveAceitarVariacaoZero()
        {
            var dto = new ProficienciaAlunoDto { Variacao = 0 };

            Assert.Equal(0, dto.Variacao);
        }
    }
}

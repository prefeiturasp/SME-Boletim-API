using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaComparativoAlunoSpDtoTeste
    {
        [Fact(DisplayName = "DTO ProficienciaComparativoAlunoSpDto deve ser criado com sucesso.")]
        public void ProficienciaComparativoAlunoSpDto_DeveSerCriadoComSucesso()
        {
            var dto = new ProficienciaComparativoAlunoSpDto();

            dto.Total = 10;
            dto.Pagina = 1;
            dto.ItensPorPagina = 5;
            dto.Aplicacoes = new List<string>();
            dto.Itens = new List<ProficienciaAlunoDto>();

            Assert.NotNull(dto);
            Assert.Equal(10, dto.Total);
            Assert.Equal(1, dto.Pagina);
            Assert.Equal(5, dto.ItensPorPagina);
            Assert.NotNull(dto.Aplicacoes);
            Assert.NotNull(dto.Itens);
        }

        [Fact(DisplayName = "DTO ProficienciaComparativoAlunoSpDto deve conter NomeDisciplina e NomeLote.")]
        public void ProficienciaComparativoAlunoSpDto_DeveConterNomeDisciplinaENomeLote()
        {
            var dto = new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024"
            };

            Assert.Equal("Matemática", dto.NomeDisciplina);
            Assert.Equal("Lote 2024", dto.NomeLote);
        }

        [Fact(DisplayName = "DTO ProficienciaComparativoAlunoSpDto deve conter UeDescricao e NomeAplicacaoPSP.")]
        public void ProficienciaComparativoAlunoSpDto_DeveConterUeDescricaoENomeAplicacaoPSP()
        {
            var dto = new ProficienciaComparativoAlunoSpDto
            {
                UeDescricao = "EMEF Teste",
                NomeAplicacaoPSP = "Prova São Paulo"
            };

            Assert.Equal("EMEF Teste", dto.UeDescricao);
            Assert.Equal("Prova São Paulo", dto.NomeAplicacaoPSP);
        }

        [Fact(DisplayName = "DTO ProficienciaComparativoAlunoSpDto deve conter itens com proficiências.")]
        public void ProficienciaComparativoAlunoSpDto_DeveConterItensComProficiencias()
        {
            var dto = new ProficienciaComparativoAlunoSpDto
            {
                Aplicacoes = new List<string> { "Abril", "Junho" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Teste",
                        Turma = "5A",
                        Variacao = 12.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 520m, NivelProficiencia = "Básico" }
                        }
                    }
                }
            };

            Assert.Equal(2, dto.Aplicacoes.Count());
            Assert.Single(dto.Itens);
            Assert.Single(dto.Itens.First().Proficiencias);
            Assert.Equal("5A", dto.Itens.First().Turma);
        }
    }
}

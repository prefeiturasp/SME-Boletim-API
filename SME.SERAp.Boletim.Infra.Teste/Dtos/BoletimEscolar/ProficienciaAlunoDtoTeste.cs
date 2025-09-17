using FluentAssertions;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaAlunoDtoTeste
    {
        [Fact(DisplayName = "DTO ProficienciaAlunoDto deve ser criado com sucesso.")]
        public void ProficienciaAlunoDto_DeveSerCriadoComSucesso()
        {
            var proficienciaAlunoDto = new ProficienciaAlunoDto();

            proficienciaAlunoDto.Nome = "Aluno Teste";
            proficienciaAlunoDto.Variacao = 15.5;
            proficienciaAlunoDto.Proficiencias = new List<ProficienciaDetalheDto>();

            proficienciaAlunoDto.Should().NotBeNull();
            proficienciaAlunoDto.Nome.Should().Be("Aluno Teste");
            proficienciaAlunoDto.Variacao.Should().Be(15.5);
            proficienciaAlunoDto.Proficiencias.Should().NotBeNull();
            proficienciaAlunoDto.Proficiencias.Should().BeEmpty();
        }
    }
}
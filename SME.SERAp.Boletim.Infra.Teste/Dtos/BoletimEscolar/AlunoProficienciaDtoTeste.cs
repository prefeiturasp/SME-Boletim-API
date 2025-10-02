using FluentAssertions;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class AlunoProficienciaDtoTeste
    {
        [Fact(DisplayName = "DTO AlunoProficienciaDto deve ser criado com sucesso.")]
        public void AlunoProficienciaDto_DeveSerCriadoComSucesso()
        {
            var alunoProficienciaDto = new AlunoProficienciaDto();

            alunoProficienciaDto.AlunoRa = 12345;
            alunoProficienciaDto.NomeAluno = "Teste de Nome";
            alunoProficienciaDto.Proficiencia = 250.5M;
            alunoProficienciaDto.LoteId = 1;
            alunoProficienciaDto.Turma = "TURMA A";
            alunoProficienciaDto.NomeAplicacao = "PSA";
            alunoProficienciaDto.Periodo = "Agosto";

            alunoProficienciaDto.Should().NotBeNull();
            alunoProficienciaDto.AlunoRa.Should().Be(12345);
            alunoProficienciaDto.NomeAluno.Should().Be("Teste de Nome");
            alunoProficienciaDto.Proficiencia.Should().Be(250.5M);
            alunoProficienciaDto.LoteId.Should().Be(1);
            alunoProficienciaDto.Turma.Should().Be("TURMA A");
            alunoProficienciaDto.NomeAplicacao.Should().Be("PSA");
            alunoProficienciaDto.Periodo.Should().Be("Agosto");
        }
    }
}
using FluentAssertions;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaComparativoAlunoSpDtoTeste
    {
        [Fact(DisplayName = "DTO ProficienciaComparativoAlunoSpDto deve ser criado com sucesso.")]
        public void ProficienciaComparativoAlunoSpDto_DeveSerCriadoComSucesso()
        {
            var comparativoDto = new ProficienciaComparativoAlunoSpDto();

            comparativoDto.Total = 10;
            comparativoDto.Pagina = 1;
            comparativoDto.ItensPorPagina = 5;
            comparativoDto.Aplicacoes = new List<string>();
            comparativoDto.Itens = new List<ProficienciaAlunoDto>();

            comparativoDto.Should().NotBeNull();
            comparativoDto.Total.Should().Be(10);
            comparativoDto.Pagina.Should().Be(1);
            comparativoDto.ItensPorPagina.Should().Be(5);
            comparativoDto.Aplicacoes.Should().NotBeNull();
            comparativoDto.Itens.Should().NotBeNull();
        }
    }
}
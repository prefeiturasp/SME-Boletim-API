using FluentAssertions;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaDetalheDtoTeste
    {
        [Fact(DisplayName = "DTO ProficienciaDetalheDto deve ser criado com sucesso.")]
        public void ProficienciaDetalheDto_DeveSerCriadoComSucesso()
        {
            var proficienciaDetalheDto = new ProficienciaDetalheDto();

            proficienciaDetalheDto.Descricao = "PSP";
            proficienciaDetalheDto.Mes = "";
            proficienciaDetalheDto.Valor = 180.25M;
            proficienciaDetalheDto.NivelProficiencia = "Avançado";

            proficienciaDetalheDto.Should().NotBeNull();
            proficienciaDetalheDto.Descricao.Should().Be("PSP");
            proficienciaDetalheDto.Mes.Should().Be("");
            proficienciaDetalheDto.Valor.Should().Be(180.25M);
            proficienciaDetalheDto.NivelProficiencia.Should().Be("Avançado");
        }
    }
}
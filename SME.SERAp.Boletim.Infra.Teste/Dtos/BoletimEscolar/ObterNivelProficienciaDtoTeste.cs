using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ObterNivelProficienciaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var disciplinaId = 101L;
            var ano = 2023;
            var descricao = "Nível Teste";
            var valorReferencia = 600;

            var dto = new ObterNivelProficienciaDto
            {
                DisciplinaId = disciplinaId,
                Ano = ano,
                Descricao = descricao,
                ValorReferencia = valorReferencia
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(ano, dto.Ano);
            Assert.Equal(descricao, dto.Descricao);
            Assert.Equal(valorReferencia, dto.ValorReferencia);
        }

        [Fact]
        public void Deve_Atribuir_ValorReferencia_Nulo()
        {
            var disciplinaId = 102L;
            var ano = 2024;
            var descricao = "Nível Teste Sem Referência";
            int? valorReferencia = null;

            var dto = new ObterNivelProficienciaDto
            {
                DisciplinaId = disciplinaId,
                Ano = ano,
                Descricao = descricao,
                ValorReferencia = valorReferencia
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(ano, dto.Ano);
            Assert.Equal(descricao, dto.Descricao);
            Assert.Null(dto.ValorReferencia);
        }
    }
}
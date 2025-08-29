using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;
using System.Linq;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.Abrangencia
{
    public class AbrangenciaUeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dreId = 10L;
            var ueId = 20L;
            var dreAbreviacao = "DRE-JA";
            var ueNome = "Escola de Teste";
            var ueTipo = TipoEscola.EMEF;

            var dto = new AbrangenciaUeDto
            {
                DreId = dreId,
                UeId = ueId,
                DreAbreviacao = dreAbreviacao,
                UeNome = ueNome,
                UeTipo = ueTipo
            };

            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(dreAbreviacao, dto.DreAbreviacao);
            Assert.Equal(ueNome, dto.UeNome);
            Assert.Equal(ueTipo, dto.UeTipo);
        }

        [Theory]
        [InlineData("DRE-JA", "Escola de Teste", TipoEscola.EMEF, "DRE JA - EMEF Escola de Teste")]
        [InlineData("DRE-JQ", "Escola Teste 2", TipoEscola.EMEI, "DRE JQ - EMEI Escola Teste 2")]
        [InlineData("DRE-JA", "Escola de Teste", TipoEscola.Nenhum, "DRE JA - Escola de Teste")]
        [InlineData(null, "Escola de Teste", TipoEscola.EMEF, "EMEF Escola de Teste")]
        [InlineData("", "Escola de Teste", TipoEscola.EMEI, "EMEI Escola de Teste")]
        [InlineData("DRE_NAO_PADRAO", "Escola Teste 3", TipoEscola.EMEF, "DRE_NAO_PADRAO - EMEF Escola Teste 3")]
        [InlineData("DRE-PV", "Escola Teste 4", TipoEscola.CEUCEMEI, "DRE PV - CEU CEMEI Escola Teste 4")]
        public void Descricao_Deve_Construir_Corretamente(string dreAbreviacao, string ueNome, TipoEscola ueTipo, string descricaoEsperada)
        {
            var dto = new AbrangenciaUeDto
            {
                DreAbreviacao = dreAbreviacao,
                UeNome = ueNome,
                UeTipo = ueTipo
            };

            var descricao = dto.Descricao;

            Assert.Equal(descricaoEsperada, descricao);
        }
    }
}
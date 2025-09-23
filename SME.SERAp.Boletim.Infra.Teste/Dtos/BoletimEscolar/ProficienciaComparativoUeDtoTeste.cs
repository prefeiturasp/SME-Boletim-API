using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaComparativoUeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var total = 50;
            var pagina = 1;
            var itensPorPagina = 10;
            var dreId = 1;
            var dreAbreviacao = "DRE-JA";
            var ues = new List<UeProficienciaDto>
            {
                new UeProficienciaDto(),
                new UeProficienciaDto()
            };

            var dto = new ProficienciaComparativoUeDto
            {
                Total = total,
                Pagina = pagina,
                ItensPorPagina = itensPorPagina,
                DreId = dreId,
                DreAbreviacao = dreAbreviacao,
                Ues = ues
            };

            Assert.Equal(total, dto.Total);
            Assert.Equal(pagina, dto.Pagina);
            Assert.Equal(itensPorPagina, dto.ItensPorPagina);
            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(dreAbreviacao, dto.DreAbreviacao);
            Assert.Equal(ues, dto.Ues);
            Assert.Equal(2, dto.Ues.Count());
        }
    }
}
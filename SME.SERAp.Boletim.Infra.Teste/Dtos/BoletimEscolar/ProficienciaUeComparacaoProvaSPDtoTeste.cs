using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaUeComparacaoProvaSPDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var totalLotes = 2;
            var proficienciaProvaSp = new ProficienciaProvaSpDto
            {
                LoteId = 1,
                NomeAplicacao = "ProvaSP",
                Periodo = "2022",
                MediaProficiencia = 600m,
                NivelProficiencia = "Nível Teste",
                TotalRealizaramProva = 100
            };
            var lotes = new List<ProficienciaUeDto>
            {
                new ProficienciaUeDto(),
                new ProficienciaUeDto()
            };

            var dto = new ProficienciaUeComparacaoProvaSPDto
            {
                TotalLotes = totalLotes,
                ProvaSP = proficienciaProvaSp,
                Lotes = lotes
            };

            Assert.Equal(totalLotes, dto.TotalLotes);
            Assert.Equal(proficienciaProvaSp, dto.ProvaSP);
            Assert.Equal(lotes, dto.Lotes);
        }
    }
}
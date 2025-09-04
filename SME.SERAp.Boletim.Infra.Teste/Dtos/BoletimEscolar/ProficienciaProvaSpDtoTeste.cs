using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaProvaSpDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 2L;
            var nomeAplicacao = "Prova SP";
            var periodo = "2022";
            var mediaProficiencia = 620.50m;
            var nivelProficiencia = "Nível Básico";
            var totalRealizaramProva = 120;

            var dto = new ProficienciaProvaSpDto
            {
                LoteId = loteId,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                MediaProficiencia = mediaProficiencia,
                NivelProficiencia = nivelProficiencia,
                TotalRealizaramProva = totalRealizaramProva
            };

            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(nivelProficiencia, dto.NivelProficiencia);
            Assert.Equal(totalRealizaramProva, dto.TotalRealizaramProva);
        }
    }
}
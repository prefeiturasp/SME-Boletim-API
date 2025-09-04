using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaUeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 1L;
            var nomeAplicacao = "Nome da Aplicação";
            var periodo = "2023-1";
            var mediaProficiencia = 650.75m;
            var nivelProficiencia = "Nível Adequado";
            var totalRealizaramProva = 150;

            var dto = new ProficienciaUeDto
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
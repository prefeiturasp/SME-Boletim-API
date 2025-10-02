using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciaDetalheUeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 123L;
            var nomeAplicacao = "Aplicação de Teste";
            var periodo = "1º Bimestre";
            var mediaProficiencia = 75.5;
            var realizaramProva = 150;
            var nivelProficiencia = "Básico";

            var dto = new ProficienciaDetalheUeDto
            {
                LoteId = loteId,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                MediaProficiencia = mediaProficiencia,
                RealizaramProva = realizaramProva,
                NivelProficiencia = nivelProficiencia
            };

            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(realizaramProva, dto.RealizaramProva);
            Assert.Equal(nivelProficiencia, dto.NivelProficiencia);
        }
    }
}
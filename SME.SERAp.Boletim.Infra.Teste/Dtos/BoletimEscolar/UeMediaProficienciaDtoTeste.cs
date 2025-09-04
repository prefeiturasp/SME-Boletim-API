using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class UeMediaProficienciaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var loteId = 1L;
            var nomeAplicacao = "Nome da Aplicação Teste";
            var periodo = "2023";
            var mediaProficiencia = 500.50m;

            var dto = new UeMediaProficienciaDto
            {
                LoteId = loteId,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                MediaProficiencia = mediaProficiencia
            };

            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
        }
    }
}
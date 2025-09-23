using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class UeProficienciaQueryResultDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var disciplinaId = 1;
            var loteId = 123L;
            var anoEscolar = 5;
            var nomeAplicacao = "Aplicação de Teste";
            var periodo = "1º Bimestre";
            var ueId = 10;
            var ueNome = "Escola de Teste";
            var dreId = 1;
            var dreAbreviacao = "DRE-JA";
            var dreNome = "Diretoria Regional de Educação";
            var tipoEscola = TipoEscola.EMEF;
            var mediaProficiencia = 75.5;
            var realizaramProva = 150;

            var dto = new UeProficienciaQueryResultDto
            {
                DisciplinaId = disciplinaId,
                LoteId = loteId,
                AnoEscolar = anoEscolar,
                NomeAplicacao = nomeAplicacao,
                Periodo = periodo,
                UeId = ueId,
                UeNome = ueNome,
                DreId = dreId,
                DreAbreviacao = dreAbreviacao,
                DreNome = dreNome,
                TipoEscola = tipoEscola,
                MediaProficiencia = mediaProficiencia,
                RealizaramProva = realizaramProva
            };

            Assert.Equal(disciplinaId, dto.DisciplinaId);
            Assert.Equal(loteId, dto.LoteId);
            Assert.Equal(anoEscolar, dto.AnoEscolar);
            Assert.Equal(nomeAplicacao, dto.NomeAplicacao);
            Assert.Equal(periodo, dto.Periodo);
            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(ueNome, dto.UeNome);
            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(dreAbreviacao, dto.DreAbreviacao);
            Assert.Equal(dreNome, dto.DreNome);
            Assert.Equal(tipoEscola, dto.TipoEscola);
            Assert.Equal(mediaProficiencia, dto.MediaProficiencia);
            Assert.Equal(realizaramProva, dto.RealizaramProva);
        }
    }
}
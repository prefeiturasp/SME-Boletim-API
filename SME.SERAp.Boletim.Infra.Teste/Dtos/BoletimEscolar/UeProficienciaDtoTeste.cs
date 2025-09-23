using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class UeProficienciaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var ueId = 10;
            var ueNome = "Escola de Teste";
            var disciplinaId = 1;
            var variacao = 5.2;

            var aplicacaoPsp = new ProficienciaDetalheUeDto
            {
                LoteId = 100L,
                NomeAplicacao = "PSP",
                Periodo = "2023",
                MediaProficiencia = 80.0,
                RealizaramProva = 100,
                NivelProficiencia = "Avançado"
            };

            var aplicacoesPsa = new List<ProficienciaDetalheUeDto>
            {
                new ProficienciaDetalheUeDto
                {
                    LoteId = 200L,
                    NomeAplicacao = "PSA 1",
                    Periodo = "2022",
                    MediaProficiencia = 75.0,
                    RealizaramProva = 95,
                    NivelProficiencia = "Básico"
                },
                new ProficienciaDetalheUeDto
                {
                    LoteId = 300L,
                    NomeAplicacao = "PSA 2",
                    Periodo = "2021",
                    MediaProficiencia = 70.0,
                    RealizaramProva = 90,
                    NivelProficiencia = "Intermediário"
                }
            };

            var dto = new UeProficienciaDto
            {
                UeId = ueId,
                UeNome = ueNome,
                Disciplinaid = disciplinaId,
                Variacao = variacao,
                AplicacaoPsp = aplicacaoPsp,
                AplicacoesPsa = aplicacoesPsa
            };

            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(ueNome, dto.UeNome);
            Assert.Equal(disciplinaId, dto.Disciplinaid);
            Assert.Equal(variacao, dto.Variacao);
            Assert.Equal(aplicacaoPsp, dto.AplicacaoPsp);
            Assert.Equal(aplicacoesPsa, dto.AplicacoesPsa);
            Assert.Equal(2, dto.AplicacoesPsa.Count());
            Assert.Equal(200L, dto.AplicacoesPsa.First().LoteId);
            Assert.Equal("PSP", dto.AplicacaoPsp.NomeAplicacao);
        }
    }
}
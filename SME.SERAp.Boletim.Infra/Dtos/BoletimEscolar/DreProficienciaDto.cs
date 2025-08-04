using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreProficienciaDto
    {
        [JsonPropertyOrder(1)]
        public long DreId { get; set; }
        [JsonPropertyOrder(2)]
        public string DreNome { get; set; }

        [JsonPropertyOrder(3)]
        public int AnoEscolar { get; set; }

        [JsonPropertyOrder(4)]
        public int TotalUes { get; set; }

        [JsonPropertyOrder(5)]
        public int TotalAlunos { get; set; }

        [JsonPropertyOrder(6)]
        public int TotalRealizaramProva { get; set; }

        [JsonPropertyOrder(7)]
        public decimal PercentualParticipacao { get; set; }

        [JsonPropertyOrder(8)]
        public IEnumerable<DisciplinaProficienciaDetalheDto> Disciplinas { get; set; }
    }
}
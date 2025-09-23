using SME.SERAp.Boletim.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeProficienciaQueryResultDto
    {
        public int DisciplinaId { get; set; }
        public long LoteId { get; set; }
        public int AnoEscolar { get; set; }
        public string NomeAplicacao { get; set; }
        public string Periodo { get; set; }
        public int UeId { get; set; }
        public string UeNome { get; set; }
        public int DreId { get; set; }
        public string DreAbreviacao { get; set; }
        public string DreNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public double MediaProficiencia { get; set; }
        public int RealizaramProva { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreNivelProficienciaDto
    {
        public long DisciplinaId { get; set; }
        public int Ano { get; set; }
        public string Descricao { get; set; }
        public int? ValorReferencia { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class ProficienciaDetalheDto
    {
        public string Descricao { get; set; }
        public string Mes { get; set; }
        public decimal Valor { get; set; }
        public string NivelProficiencia { get; set; }
    }
}
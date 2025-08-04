using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class DreDto
    {
        public long DreId { get; set; }
        public string DreNome { get; set; }
        public string DreNomeAbreviado { get; set; }
    }
}
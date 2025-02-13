using SME.SERAp.Boletim.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Autenticacao
{
    public class ObterAlunoAtivoRetornoDto
    {
        public long Ra { get; set; }
        public string Ano { get; set; }
        public int TipoTurno { get; set; }
        public Modalidade Modalidade { get; set; }
        public long? TurmaId { get; set; }

    }
}
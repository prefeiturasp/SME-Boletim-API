using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeProficienciaDto
    {
        public int UeId { get; set; }
        public string UeNome { get; set; }
        public int Disciplinaid { get; set; }
        public double Variacao { get; set; }
        public ProficienciaDetalheUeDto AplicacaoPsp { get; set; }
        public IEnumerable<ProficienciaDetalheUeDto> AplicacoesPsa { get; set; }
    }
}
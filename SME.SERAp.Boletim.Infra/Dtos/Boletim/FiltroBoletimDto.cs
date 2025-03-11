using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroBoletimDto
    {
            public List<int> Ano { get; set; } = new();
            public List<string> ComponentesCurriculares { get; set; } = new();
        
    }
}

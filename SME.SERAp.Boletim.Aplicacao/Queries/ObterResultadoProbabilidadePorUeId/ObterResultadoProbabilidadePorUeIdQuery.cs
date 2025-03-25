using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId
{
    public class ObterResultadoProbabilidadePorUeIdQuery : IRequest<IEnumerable<ResultadoProbabilidadeDto>>
    {
        public ObterResultadoProbabilidadePorUeIdQuery(long ueId, long disciplinaId, int anoEscolar)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }

        public long UeId { get; set; }
        public long DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
    }
}
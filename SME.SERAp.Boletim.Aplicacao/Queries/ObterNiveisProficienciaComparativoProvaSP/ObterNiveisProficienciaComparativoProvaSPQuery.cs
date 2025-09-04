using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP
{
    public class ObterNiveisProficienciaComparativoProvaSPQuery : IRequest<ProficienciaUeComparacaoProvaSPDto>
    {
        public long LoteId { get; }
        public int UeId { get; }
        public int DisciplinaId { get; }
        public int AnoEscolar { get; }
        
        public ObterNiveisProficienciaComparativoProvaSPQuery(long loteId, int ueId, int disciplinaId, int anoEscolar)
        {
            LoteId = loteId;
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }
}
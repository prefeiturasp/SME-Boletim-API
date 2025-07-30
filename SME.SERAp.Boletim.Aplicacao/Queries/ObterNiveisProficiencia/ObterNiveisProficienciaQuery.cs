using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia
{
    public class ObterNiveisProficienciaQuery : IRequest<IEnumerable<NivelProficienciaDto>>
    {
        public int AnoEscolar { get; }
        public long LoteId { get; }

        public ObterNiveisProficienciaQuery(int anoEscolar, long loteId)
        {
            AnoEscolar = anoEscolar;
            LoteId = loteId;
        }
    }
}
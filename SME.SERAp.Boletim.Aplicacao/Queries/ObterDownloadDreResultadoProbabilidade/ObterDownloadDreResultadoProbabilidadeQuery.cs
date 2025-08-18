using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadDreResultadoProbabilidade
{
    public class ObterDownloadDreResultadoProbabilidadeQuery : IRequest<IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        public long LoteId { get; set; }
        public int DreId { get; set; }

        public ObterDownloadDreResultadoProbabilidadeQuery(long loteId, int dreId)
        {
            LoteId = loteId;
            DreId = dreId;
        }
    }
}
using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolarPorDre
{
    public class ObterDownloadProvasBoletimEscolarPorDreQuery : IRequest<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>>
    {
        public ObterDownloadProvasBoletimEscolarPorDreQuery(long dreId, long loteId)
        {
            DreId = dreId;
            LoteId = loteId;
        }

        public long DreId { get; }
        public long LoteId { get; }
    }
}
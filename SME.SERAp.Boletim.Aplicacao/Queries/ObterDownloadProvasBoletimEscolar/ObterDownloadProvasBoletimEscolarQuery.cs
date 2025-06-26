using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar
{
    public class ObterDownloadProvasBoletimEscolarQuery : IRequest<IEnumerable<DownloadProvasBoletimEscolarDto>>
    {
        public ObterDownloadProvasBoletimEscolarQuery(long loteId, long ueId)
        {
            LoteId = loteId;
            UeId = ueId;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
    }
}

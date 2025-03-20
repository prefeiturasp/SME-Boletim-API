using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar
{
    public class ObterDownloadProvasBoletimEscolarQuery : IRequest<IEnumerable<DownloadProvasBoletimEscolarDto>>
    {
        public ObterDownloadProvasBoletimEscolarQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}

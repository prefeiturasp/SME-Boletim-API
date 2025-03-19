using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar
{
    public class ObterDownloadProvasBoletimEscolarQuery : IRequest<IEnumerable<DownloadProvasBoletimEscolarDto>>
    {
        public ObterDownloadProvasBoletimEscolarQuery(string ueId)
        {
            UeId = ueId;
        }

        public string UeId { get; set; }
    }
}

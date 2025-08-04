using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDownloadProvasBoletimEscolarSmeQuery : IRequest<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>>
    {
        public long LoteId { get; set; }

        public ObterDownloadProvasBoletimEscolarSmeQuery(long loteId)
        {
            LoteId = loteId;
        }
    }
}

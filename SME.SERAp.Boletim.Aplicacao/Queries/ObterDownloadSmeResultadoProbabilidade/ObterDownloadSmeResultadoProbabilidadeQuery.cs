using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDownloadSmeResultadoProbabilidadeQuery : IRequest<IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        public long LoteId { get; set; }

        public ObterDownloadSmeResultadoProbabilidadeQuery(long loteId)
        {
            LoteId = loteId;
        }
    }
}

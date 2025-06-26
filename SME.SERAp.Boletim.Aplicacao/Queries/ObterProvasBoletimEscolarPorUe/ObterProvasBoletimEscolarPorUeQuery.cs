using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe
{
    public class ObterProvasBoletimEscolarPorUeQuery : IRequest<IEnumerable<ProvaBoletimEscolarDto>>
    {
        public ObterProvasBoletimEscolarPorUeQuery(long loteId, long ueId, FiltroBoletimDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            Filtros = filtros;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
        public FiltroBoletimDto Filtros { get; set; }
    }
}

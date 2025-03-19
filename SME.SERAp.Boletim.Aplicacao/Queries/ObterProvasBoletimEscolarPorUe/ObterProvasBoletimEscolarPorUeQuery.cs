using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe
{
    public class ObterProvasBoletimEscolarPorUeQuery : IRequest<IEnumerable<ProvaBoletimEscolarDto>>
    {
        public ObterProvasBoletimEscolarPorUeQuery(long ueId, FiltroBoletimDto filtros)
        {
            UeId = ueId;
            Filtros = filtros;
        }

        public long UeId { get; set; }
        public FiltroBoletimDto Filtros { get; set; }
    }
}

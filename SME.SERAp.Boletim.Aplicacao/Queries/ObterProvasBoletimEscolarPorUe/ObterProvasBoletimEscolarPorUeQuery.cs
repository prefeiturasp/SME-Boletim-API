using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe
{
    public class ObterProvasBoletimEscolarPorUeQuery : IRequest<IEnumerable<ProvaBoletimEscolarDto>>
    {
        public ObterProvasBoletimEscolarPorUeQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}

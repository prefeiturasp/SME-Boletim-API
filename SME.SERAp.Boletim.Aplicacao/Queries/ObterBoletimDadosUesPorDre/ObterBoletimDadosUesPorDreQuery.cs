using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimDadosUesPorDre
{
    public class ObterBoletimDadosUesPorDreQuery : IRequest<PaginacaoUesBoletimDadosDto>
    {
        public long LoteId { get; set; }
        public long DreId { get; set; }
        public int AnoEscolar { get; set; }
        public FiltroUeBoletimDadosDto Filtros { get; set; }

        public ObterBoletimDadosUesPorDreQuery(long loteId, long dreId, int anoEscolar, FiltroUeBoletimDadosDto filtros)
        {
            LoteId = loteId;
            DreId = dreId;
            AnoEscolar = anoEscolar;
            Filtros = filtros;
        }
    }
}

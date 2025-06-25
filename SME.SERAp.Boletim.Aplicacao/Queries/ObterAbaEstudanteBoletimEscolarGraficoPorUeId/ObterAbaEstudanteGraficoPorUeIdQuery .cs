using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId
{
    public class ObterAbaEstudanteGraficoPorUeIdQuery : IRequest<IEnumerable<AbaEstudanteGraficoDto>>
    {
        public long LoteId { get; set; }
        public long UeId { get; set; }

        public FiltroBoletimEstudanteDto Filtros { get; set; }

        public ObterAbaEstudanteGraficoPorUeIdQuery(long loteId, long ueId, FiltroBoletimEstudanteDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            Filtros = filtros;
        }
    }
}
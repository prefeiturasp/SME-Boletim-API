using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId
{
    public class ObterAbaEstudanteBoletimEscolarPorUeIdQuery
    : IRequest<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
    {
        public ObterAbaEstudanteBoletimEscolarPorUeIdQuery(long loteId, long ueId, FiltroBoletimEstudantePaginadoDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            Filtros = filtros;
        }

        public long LoteId { get; set; }
        public long UeId { get; set; }
        public FiltroBoletimEstudantePaginadoDto Filtros { get; set; }
    }
}
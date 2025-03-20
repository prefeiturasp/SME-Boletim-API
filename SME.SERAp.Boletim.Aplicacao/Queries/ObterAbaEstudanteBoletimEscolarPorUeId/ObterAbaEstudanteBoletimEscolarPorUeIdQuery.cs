using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId
{
    class ObterAbaEstudanteBoletimEscolarPorUeIdQuery
    : IRequest<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
    {
        public ObterAbaEstudanteBoletimEscolarPorUeIdQuery(long ueId, FiltroBoletimEstudantePaginadoDto filtros)
        {
            UeId = ueId;
            Filtros = filtros;
        }

        public long UeId { get; set; }
        public FiltroBoletimEstudantePaginadoDto Filtros { get; set; }
    }
}
using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadeListaPorUeId
{
    public class ObterResultadoProbabilidadeListaPorUeIdQuery : IRequest<(IEnumerable<ResultadoProbabilidadeDto>, int)>
    {
        public long UeId { get; }
        public long DisciplinaId { get; }
        public int AnoEscolar { get; }
        public FiltroBoletimResultadoProbabilidadeDto Filtros { get; set; }

        public ObterResultadoProbabilidadeListaPorUeIdQuery(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            Filtros = filtros;
        }
    }
}

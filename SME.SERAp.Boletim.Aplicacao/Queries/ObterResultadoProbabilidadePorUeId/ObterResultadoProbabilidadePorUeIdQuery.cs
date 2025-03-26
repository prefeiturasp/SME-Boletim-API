using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId
{
    public class ObterResultadoProbabilidadePorUeIdQuery : IRequest<(IEnumerable<ResultadoProbabilidadeDto>, int)>
    {
        public long UeId { get; }
        public long ProvaId { get; }
        public long DisciplinaId { get; }
        public int AnoEscolar { get; }
        public FiltroBoletimResultadoProbabilidadeDto Filtros { get; set; }

        public ObterResultadoProbabilidadePorUeIdQuery(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            Filtros = filtros;
        }
    }
}
using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaUes
{
    public class ObterNiveisProficienciaUesQuery : IRequest<IEnumerable<UeNivelProficienciaDto>>
    {
        public long DreId { get; }
        public int AnoEscolar { get; }
        public long LoteId { get; }

        public ObterNiveisProficienciaUesQuery(long dreId, int anoEscolar, long loteId)
        {
            DreId = dreId;
            AnoEscolar = anoEscolar;
            LoteId = loteId;
        }
    }
}

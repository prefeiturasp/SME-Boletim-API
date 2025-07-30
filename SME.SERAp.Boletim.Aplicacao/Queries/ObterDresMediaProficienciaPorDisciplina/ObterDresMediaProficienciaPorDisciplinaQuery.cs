using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDresMediaProficienciaPorDisciplinaQuery : IRequest<IEnumerable<DreDisciplinaMediaProficienciaDto>>
    {
        public ObterDresMediaProficienciaPorDisciplinaQuery(long loteId, int anoEscolar, IEnumerable<long> dresIds)
        {
            LoteId = loteId;
            AnoEscolar = anoEscolar;
            DresIds = dresIds;
        }

        public long LoteId { get; }
        public int AnoEscolar { get; }
        public IEnumerable<long> DresIds { get; set; }
    }
}

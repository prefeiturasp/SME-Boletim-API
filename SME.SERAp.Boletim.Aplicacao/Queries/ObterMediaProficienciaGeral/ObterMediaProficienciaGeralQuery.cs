using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterMediaProficienciaGeralQuery : IRequest<IEnumerable<MediaProficienciaDisciplinaDto>>
    {
        public ObterMediaProficienciaGeralQuery(long loteId, int anoEscolar)
        {
            LoteId = loteId;
            AnoEscolar = anoEscolar;
        }
        public long LoteId { get; }
        public int AnoEscolar { get; }
    }
}

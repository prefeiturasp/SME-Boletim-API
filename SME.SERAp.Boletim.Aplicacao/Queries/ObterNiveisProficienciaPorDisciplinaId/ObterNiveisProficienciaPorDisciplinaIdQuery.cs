using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId
{
    public class ObterNiveisProficienciaPorDisciplinaIdQuery : IRequest<IEnumerable<ObterNivelProficienciaDto>>
    {
        public ObterNiveisProficienciaPorDisciplinaIdQuery(int disciplinaId, int anoEscolar)
        {
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
        public int DisciplinaId { get; }
        public int AnoEscolar { get; }
    }
}

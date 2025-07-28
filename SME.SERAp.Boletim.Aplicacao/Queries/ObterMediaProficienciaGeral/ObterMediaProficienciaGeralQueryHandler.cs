using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterMediaProficienciaGeralQueryHandler : IRequestHandler<ObterMediaProficienciaGeralQuery, IEnumerable<MediaProficienciaDisciplinaDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterMediaProficienciaGeralQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }
        public Task<IEnumerable<MediaProficienciaDisciplinaDto>> Handle(ObterMediaProficienciaGeralQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterMediaProficienciaGeral(request.LoteId, request.AnoEscolar);
        }
    }
}

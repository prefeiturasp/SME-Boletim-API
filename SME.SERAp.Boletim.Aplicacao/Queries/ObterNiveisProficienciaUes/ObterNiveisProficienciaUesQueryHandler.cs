using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaUes
{
    public class ObterNiveisProficienciaUesQueryHandler : IRequestHandler<ObterNiveisProficienciaUesQuery, IEnumerable<UeNivelProficienciaDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterNiveisProficienciaUesQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public async Task<IEnumerable<UeNivelProficienciaDto>> Handle(ObterNiveisProficienciaUesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimProvaAluno.ObterNiveisProficienciaUes(request.DreId, request.AnoEscolar, request.LoteId);
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId
{
    public class ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQueryHandler
        : IRequestHandler<ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery, IEnumerable<NivelProficienciaBoletimEscolarDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> Handle(ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(request.UeId, request.ProvaId, request.Filtros);
        }
    }
}

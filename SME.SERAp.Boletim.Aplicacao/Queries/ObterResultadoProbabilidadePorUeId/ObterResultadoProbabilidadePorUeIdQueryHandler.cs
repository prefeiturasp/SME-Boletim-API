using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId
{
    public class ObterResultadoProbabilidadePorUeIdQueryHandler : IRequestHandler<ObterResultadoProbabilidadePorUeIdQuery, IEnumerable<ResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;

        public ObterResultadoProbabilidadePorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimResultadoProbabilidade;
        }

        public async Task<IEnumerable<ResultadoProbabilidadeDto>> Handle(ObterResultadoProbabilidadePorUeIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimProvaAluno.ObterResultadoProbabilidadePorUeAsync(request.UeId, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

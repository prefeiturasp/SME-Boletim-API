using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia
{
    public class ObterNiveisProficienciaQueryHandler : IRequestHandler<ObterNiveisProficienciaQuery, IEnumerable<NivelProficienciaDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterNiveisProficienciaQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public async Task<IEnumerable<NivelProficienciaDto>> Handle(ObterNiveisProficienciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimProvaAluno.ObterNiveisProficienciaDisciplinas(request.AnoEscolar, request.LoteId);
        }
    }
}
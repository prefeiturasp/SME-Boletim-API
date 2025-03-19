using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId
{
    public class ObterAbaEstudanteGraficoPorUeIdQueryHandler : IRequestHandler<ObterAbaEstudanteGraficoPorUeIdQuery, IEnumerable<AbaEstudanteGraficoDto>>
    {
        private readonly IRepositorioBoletimProvaAluno _repositorioBoletimProvaAluno;

        public ObterAbaEstudanteGraficoPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            _repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> Handle(ObterAbaEstudanteGraficoPorUeIdQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioBoletimProvaAluno.ObterAbaEstudanteGraficoPorUeId(request.UeId);
        }
    }
}
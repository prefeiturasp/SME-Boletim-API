using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId
{
    class ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandler
    : IRequestHandler<ObterAbaEstudanteBoletimEscolarPorUeIdQuery, (IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterAbaEstudanteBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
            Handle(ObterAbaEstudanteBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterAbaEstudanteBoletimEscolarPorUeId(request.UeId, request.Filtros);
        }
    }
}
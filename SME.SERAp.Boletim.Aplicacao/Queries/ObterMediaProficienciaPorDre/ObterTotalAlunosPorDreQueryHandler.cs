using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre
{
    public class ObterTotalAlunosPorDreQueryHandler : IRequestHandler<ObterTotalAlunosPorDreQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterTotalAlunosPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int> Handle(ObterTotalAlunosPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalAlunosPorDreAsync(request.LoteId, request.DreId, request.AnoEscolar);
        }
    }
}
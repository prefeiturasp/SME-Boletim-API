using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre
{
    public class ObterTotalUesPorDreQueryHandler : IRequestHandler<ObterTotalUesPorDreQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterTotalUesPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int> Handle(ObterTotalUesPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterTotalUesPorDreAsync(request.LoteId, request.DreId, request.AnoEscolar);
        }
    }
}
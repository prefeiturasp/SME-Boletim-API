using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre
{
    public class ObterUesPorDreQueryHandler : IRequestHandler<ObterUesPorDreQuery, IEnumerable<UePorDreDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterUesPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<UePorDreDto>> Handle(ObterUesPorDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterUesPorDreAsync(request.DreId, request.AnoEscolar, request.LoteId);
        }
    }
}
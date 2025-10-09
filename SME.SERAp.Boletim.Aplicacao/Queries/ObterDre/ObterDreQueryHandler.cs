using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDre
{
    public class ObterDreQueryHandler : IRequestHandler<ObterDrePorAnoEscolarLoteIdQuery, IEnumerable<DreDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<DreDto>> Handle(ObterDrePorAnoEscolarLoteIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDreAsync(request.AnoEscolar, request.LoteId);
        }
    }
}
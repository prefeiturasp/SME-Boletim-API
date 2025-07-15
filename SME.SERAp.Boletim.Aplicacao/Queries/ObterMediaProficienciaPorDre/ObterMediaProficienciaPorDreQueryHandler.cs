using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre
{
    public class ObterMediaProficienciaPorDreQueryHandler : IRequestHandler<ObterMediaProficienciaPorDreQuery, IEnumerable<MediaProficienciaDisciplinaDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterMediaProficienciaPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<MediaProficienciaDisciplinaDto>> Handle(ObterMediaProficienciaPorDreQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterMediaProficienciaPorDreAsync(request.LoteId, request.DreId, request.AnoEscolar);
        }
    }
}
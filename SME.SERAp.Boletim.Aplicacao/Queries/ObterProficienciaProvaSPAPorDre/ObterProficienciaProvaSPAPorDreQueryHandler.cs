using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre
{
    public class ObterProficienciaProvaSPAPorDreQueryHandler : IRequestHandler<ObterProficienciaProvaSPAPorDreQuery, IEnumerable<ResultadoProeficienciaPorDre>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterProficienciaProvaSPAPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<ResultadoProeficienciaPorDre>> Handle(ObterProficienciaProvaSPAPorDreQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterProficienciaPorDreProvaSPAsync(request.DreId, request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
    }
}
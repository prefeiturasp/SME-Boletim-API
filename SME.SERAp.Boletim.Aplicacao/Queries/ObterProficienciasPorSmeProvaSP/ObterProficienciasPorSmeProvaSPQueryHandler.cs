using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP
{
    public class ObterProficienciasPorSmeProvaSPQueryHandler : IRequestHandler<ObterProficienciasPorSmeProvaSPQuery, IEnumerable<ResultadoProeficienciaPorDre>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterProficienciasPorSmeProvaSPQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<ResultadoProeficienciaPorDre>> Handle(ObterProficienciasPorSmeProvaSPQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterProficienciasPorSmeProvaSPAsync(request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
    }
}
using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes
{
    internal class ObterProficienciaPorSmeProvaSaberesQueryHandler : IRequestHandler<ObterProficienciaPorSmeProvaSaberesQuery, IEnumerable<ResultadoProeficienciaPorDre>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterProficienciaPorSmeProvaSaberesQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<ResultadoProeficienciaPorDre>> Handle(ObterProficienciaPorSmeProvaSaberesQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterProficienciaPorSmeProvaSaberesAsync(request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
    }
}

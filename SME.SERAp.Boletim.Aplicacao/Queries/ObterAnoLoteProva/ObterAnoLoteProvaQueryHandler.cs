using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva
{
    public class ObterAnoLoteProvaQueryHandler : IRequestHandler<ObterAnoLoteProvaQuery, int>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterAnoLoteProvaQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<int> Handle(ObterAnoLoteProvaQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterAnoPorLoteIdAsync(request.LoteId);
        }
    }
}
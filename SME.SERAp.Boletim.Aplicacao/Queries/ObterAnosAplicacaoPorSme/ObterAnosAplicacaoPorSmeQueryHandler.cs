using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosAplicacaoPorSme
{
    public class ObterAnosAplicacaoPorSmeQueryHandler : IRequestHandler<ObterAnosAplicacaoPorSmeQuery, IEnumerable<int>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorio;

        public ObterAnosAplicacaoPorSmeQueryHandler(IRepositorioBoletimProvaAluno repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<int>> Handle(ObterAnosAplicacaoPorSmeQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnosAplicacaoPorSme();
        }
    }
}
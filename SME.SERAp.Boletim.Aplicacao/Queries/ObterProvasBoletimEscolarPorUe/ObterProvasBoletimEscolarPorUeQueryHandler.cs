using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe
{
    public class ObterProvasBoletimEscolarPorUeQueryHandler
        : IRequestHandler<ObterProvasBoletimEscolarPorUeQuery, IEnumerable<ProvaBoletimEscolarDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterProvasBoletimEscolarPorUeQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<IEnumerable<ProvaBoletimEscolarDto>> Handle(ObterProvasBoletimEscolarPorUeQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.ObterProvasBoletimEscolarPorUe(request.UeId, request.Filtros);
        }
    }
}

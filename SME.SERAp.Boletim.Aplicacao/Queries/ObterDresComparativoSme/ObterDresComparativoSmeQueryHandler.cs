using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme
{
    public class ObterDresComparativoSmeQueryHandler : IRequestHandler<ObterDresComparativoSmeQuery, IEnumerable<DreDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterDresComparativoSmeQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar ?? throw new ArgumentNullException(nameof(repositorioBoletimEscolar));
        }

        public async Task<IEnumerable<DreDto>> Handle(ObterDresComparativoSmeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimEscolar.ObterDresComparativoSmeAsync(request.AnoAplicacao, request.DisciplinaId, request.AnoEscolar);
        }
    }
}

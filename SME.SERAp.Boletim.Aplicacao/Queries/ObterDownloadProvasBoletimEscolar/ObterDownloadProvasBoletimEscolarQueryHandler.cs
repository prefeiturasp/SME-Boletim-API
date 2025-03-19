using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar
{
    public class ObterDownloadProvasBoletimEscolarQueryHandler : IRequestHandler<ObterDownloadProvasBoletimEscolarQuery, IEnumerable<DownloadProvasBoletimEscolarDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;

        public ObterDownloadProvasBoletimEscolarQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarDto>> Handle(ObterDownloadProvasBoletimEscolarQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimEscolar.ObterDownloadProvasBoletimEscolar(request.UeId);
        }
    }
}

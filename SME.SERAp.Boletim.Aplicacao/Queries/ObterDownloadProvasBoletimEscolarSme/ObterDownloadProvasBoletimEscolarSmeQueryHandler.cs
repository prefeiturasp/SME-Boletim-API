using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDownloadProvasBoletimEscolarSmeQueryHandler : IRequestHandler<ObterDownloadProvasBoletimEscolarSmeQuery, IEnumerable<DownloadProvasBoletimEscolarPorDreDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterDownloadProvasBoletimEscolarSmeQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> Handle(ObterDownloadProvasBoletimEscolarSmeQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterDownloadProvasBoletimEscolarSme(request.LoteId);
    }
}

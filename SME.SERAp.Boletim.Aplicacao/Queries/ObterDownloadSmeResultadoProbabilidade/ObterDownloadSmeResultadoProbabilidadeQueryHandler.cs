using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterDownloadSmeResultadoProbabilidadeQueryHandler : IRequestHandler<ObterDownloadSmeResultadoProbabilidadeQuery, IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterDownloadSmeResultadoProbabilidadeQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<IEnumerable<DownloadResultadoProbabilidadeDto>> Handle(ObterDownloadSmeResultadoProbabilidadeQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterDownloadSmeResultadoProbabilidade(request.LoteId);
        }
    }
}

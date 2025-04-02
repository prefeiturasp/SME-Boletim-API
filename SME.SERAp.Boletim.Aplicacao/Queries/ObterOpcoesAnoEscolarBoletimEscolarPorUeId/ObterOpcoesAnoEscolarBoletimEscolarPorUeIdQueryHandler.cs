using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId
{
    public class ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQueryHandler
        : IRequestHandler<ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
            this.repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCacheOpcoesFiltrosAnoEscolar = string
                .Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosAnoEscolar, request.UeId);
            return await repositorioCache
                .ObterRedisAsync(chaveCacheOpcoesFiltrosAnoEscolar, async () => await repositorioBoletimProvaAluno.ObterOpcoesAnoEscolarBoletimEscolarPorUeId(request.UeId));
        }
    }
}

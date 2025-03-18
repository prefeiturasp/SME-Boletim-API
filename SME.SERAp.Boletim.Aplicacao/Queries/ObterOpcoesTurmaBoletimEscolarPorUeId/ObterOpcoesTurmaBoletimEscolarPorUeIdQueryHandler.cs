using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId
{
    public class ObterOpcoesTurmaBoletimEscolarPorUeIdQueryHandler
        : IRequestHandler<ObterOpcoesTurmaBoletimEscolarPorUeIdQuery, IEnumerable<OpcaoFiltroDto<string>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterOpcoesTurmaBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
            this.repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<string>>> Handle(ObterOpcoesTurmaBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCacheOpcoesFiltrosTurma = string
                .Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosTurma, request.UeId);
            return await repositorioCache
                .ObterRedisAsync(chaveCacheOpcoesFiltrosTurma, async () => await repositorioBoletimProvaAluno.ObterOpcoesTurmaBoletimEscolarPorUeId(request.UeId));
        }
    }
}

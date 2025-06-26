using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId
{
    public class ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQueryHandler
        : IRequestHandler<ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
            this.repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCacheOpcoesFiltrosComponenteCurricular = string
                .Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosComponenteCurricular, request.LoteId, request.UeId);
            return await repositorioCache
                .ObterRedisAsync(chaveCacheOpcoesFiltrosComponenteCurricular, async () => await repositorioBoletimProvaAluno.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(request.LoteId, request.UeId));
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId
{
    public class ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQueryHandler
        : IRequestHandler<ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery, IEnumerable<OpcaoFiltroDto<int>>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
            this.repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Handle(ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCacheOpcoesFiltrosNivelProficiencia = string
                .Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosNivelProficiencia, request.UeId);
            return await repositorioCache
                .ObterRedisAsync(chaveCacheOpcoesFiltrosNivelProficiencia, async () => await repositorioBoletimProvaAluno.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(request.UeId));
        }
    }
}

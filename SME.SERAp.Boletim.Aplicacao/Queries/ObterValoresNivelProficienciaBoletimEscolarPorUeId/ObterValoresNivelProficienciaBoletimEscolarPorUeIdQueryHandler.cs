using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId
{
    public class ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandler
        : IRequestHandler<ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery, BoletimEscolarValoresNivelProficienciaDto>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterValoresNivelProficienciaBoletimEscolarPorUeIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
            this.repositorioCache = repositorioCache;
        }

        public async Task<BoletimEscolarValoresNivelProficienciaDto> Handle(ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCacheOpcoesFiltrosValorProficiencia = string
                .Format(CacheChave.BolemtimEscolarUeOpcoesFiltrosValorProficiencia, request.UeId);
            return await repositorioCache
                .ObterRedisAsync(chaveCacheOpcoesFiltrosValorProficiencia, async () => await repositorioBoletimProvaAluno.ObterValoresNivelProficienciaBoletimEscolarPorUeId(request.UeId));
        }
    }
}

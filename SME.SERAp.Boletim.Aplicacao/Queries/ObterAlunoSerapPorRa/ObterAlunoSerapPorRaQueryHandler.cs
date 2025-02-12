using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa
{
    public class ObterAlunoSerapPorRaQueryHandler : IRequestHandler<ObterAlunoSerapPorRaQuery, Aluno>
    {
        private readonly IRepositorioAluno repositorioAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterAlunoSerapPorRaQueryHandler(IRepositorioAluno repositorioAluno, IRepositorioCache repositorioCache)
        {
            this.repositorioAluno = repositorioAluno ?? throw new System.ArgumentNullException(nameof(repositorioAluno));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Aluno> Handle(ObterAlunoSerapPorRaQuery request, CancellationToken cancellationToken)
        {
            var chaveAlunoRa = string.Format(CacheChave.AlunoRa, request.AlunoRA);

            return await repositorioCache.ObterRedisAsync(chaveAlunoRa, async () => await repositorioAluno.ObterPorRA(request.AlunoRA));
        }
    }
}
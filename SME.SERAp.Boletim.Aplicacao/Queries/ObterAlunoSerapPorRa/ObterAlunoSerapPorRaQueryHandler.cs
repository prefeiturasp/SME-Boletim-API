using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
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

        public ObterAlunoSerapPorRaQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new System.ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<Aluno> Handle(ObterAlunoSerapPorRaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAluno.ObterPorRA(request.AlunoRA);
        }
    }
}
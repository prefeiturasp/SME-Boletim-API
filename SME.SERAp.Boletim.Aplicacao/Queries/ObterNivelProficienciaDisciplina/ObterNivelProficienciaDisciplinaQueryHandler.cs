using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina
{
    public class ObterNivelProficienciaDisciplinaQueryHandler : IRequestHandler<ObterNivelProficienciaDisciplinaQuery, string>
    {
        public ObterNivelProficienciaDisciplinaQueryHandler()
        {
        }

        public Task<string> Handle(ObterNivelProficienciaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            var niveisDaDisciplina = request.Niveis.Where(n => n.DisciplinaId == request.DisciplinaId).OrderBy(n => n.ValorReferencia).ToList();

            foreach (var nivel in niveisDaDisciplina)
            {
                if (nivel.ValorReferencia.HasValue && request.Media <= nivel.ValorReferencia.Value)
                {
                    return Task.FromResult(nivel.Descricao);
                }
            }

            return Task.FromResult(niveisDaDisciplina.FirstOrDefault(n => !n.ValorReferencia.HasValue)?.Descricao ?? "Nível não definido");
        }
    }
}
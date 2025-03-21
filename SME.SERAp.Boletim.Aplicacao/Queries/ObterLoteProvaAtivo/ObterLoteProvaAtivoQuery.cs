using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLoteProvaAtivo
{
    public class ObterLoteProvaAtivoQuery : IRequest<LoteProvaAtivoDto>
    {
    }
}

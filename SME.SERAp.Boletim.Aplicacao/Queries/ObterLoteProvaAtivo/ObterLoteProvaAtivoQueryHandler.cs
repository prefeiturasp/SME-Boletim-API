using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLoteProvaAtivo
{
    public class ObterLoteProvaAtivoQueryHandler
        : IRequestHandler<ObterLoteProvaAtivoQuery, LoteProvaAtivoDto>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public ObterLoteProvaAtivoQueryHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<LoteProvaAtivoDto> Handle(ObterLoteProvaAtivoQuery request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.ObterLoteProvaAtivo();
        }
    }
}

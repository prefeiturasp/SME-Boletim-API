using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLoteProvaAtivo;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimNomeAplicacaoProvaUseCase : IObterBoletimNomeAplicacaoProvaUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimNomeAplicacaoProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public  Task<LoteProvaAtivoDto> Executar()
        {
            return mediator.Send(new ObterLoteProvaAtivoQuery());
        }
    }
}

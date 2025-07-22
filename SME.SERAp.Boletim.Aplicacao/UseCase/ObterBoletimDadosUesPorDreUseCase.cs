using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimDadosUesPorDre;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimDadosUesPorDreUseCase : IObterBoletimDadosUesPorDreUseCase
    {
        private readonly IMediator mediator;
        public ObterBoletimDadosUesPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<PaginacaoUesBoletimDadosDto> Executar(long loteId, long dreId, int anoEscolar, FiltroUeBoletimDadosDto filtros)
        {
            return mediator.Send(new ObterBoletimDadosUesPorDreQuery(loteId, dreId, anoEscolar, filtros));
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAbaEstudanteGraficoPorUeIdUseCase : IObterAbaEstudanteGraficoPorUeIdUseCase
    {
        private readonly IMediator _mediator;

        public ObterAbaEstudanteGraficoPorUeIdUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> Executar(long loteId, long ueId, FiltroBoletimEstudanteDto filtros)
        {
            var abrangenciasUsuarioLogado = await _mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            return await _mediator.Send(new ObterAbaEstudanteGraficoPorUeIdQuery(loteId, ueId, filtros));
        }
    }
}
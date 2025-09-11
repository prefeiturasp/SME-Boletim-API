using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterTurmasUeAnoUseCase : IObterTurmasUeAnoUseCase
    {
        private readonly IMediator mediator;
        public ObterTurmasUeAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<TurmaAnoDto>> Executar(long loteId, long ueId, int disciplinaId, int anoEscolar)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            return await mediator.Send(new ObterTurmasUeAnoQuery(loteId, ueId, disciplinaId, anoEscolar));
        }
    }
}

using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.RemoverCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class AutenticacaoValidarUseCase : AbstractUseCase, IAutenticacaoValidarUseCase
    {
        public AutenticacaoValidarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AutenticacaoRetornoDto> Executar(AutenticacaoValidarDto autenticacaoValidarDto)
        {
            var abrangencias = await mediator.Send(new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(autenticacaoValidarDto.Codigo));

            if (abrangencias == null || !abrangencias.Any())
                throw new NaoAutorizadoException("Código inválido", 401);

            await mediator.Send(new RemoverCodigoValidacaoAutenticacaoCommand(autenticacaoValidarDto.Codigo));

            return await mediator.Send(new ObterTokenJwtQuery(abrangencias));
        }
    }
}

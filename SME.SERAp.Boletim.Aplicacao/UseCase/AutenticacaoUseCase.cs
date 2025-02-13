using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class AutenticacaoUseCase : AbstractUseCase, IAutenticacaoUseCase
    {

        public AutenticacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AutenticacaoValidarDto> Executar(AutenticacaoDto autenticacaoDto)
        {
            var abrangencias = await mediator.Send(new ObterAbrangenciaPorLoginGrupoQuery(autenticacaoDto.Login, autenticacaoDto.GrupoId));

            if (abrangencias == null || !abrangencias.Any())
                throw new NaoAutorizadoException($"Usuário: {autenticacaoDto.Login} inválido com o grupo {autenticacaoDto.GrupoId}.", 401);

            return await mediator.Send(new GerarCodigoValidacaoAutenticacaoCommand(abrangencias));
        }

    }
}

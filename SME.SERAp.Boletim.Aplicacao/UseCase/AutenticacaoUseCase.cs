using Elastic.Apm.Api;
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
        private const string ChaveEnvironmentVariableName = "ChaveSerapProvaApi";
        public AutenticacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AutenticacaoValidarDto> Executar(AutenticacaoDto autenticacaoDto)
        {
            var chaveApi = Environment.GetEnvironmentVariable(ChaveEnvironmentVariableName);
            if (!string.IsNullOrWhiteSpace(chaveApi))
            {
                if (string.IsNullOrWhiteSpace(autenticacaoDto.ChaveApi) || !autenticacaoDto.ChaveApi.Equals(chaveApi))
                    throw new NaoAutorizadoException($"Chave api: {autenticacaoDto.ChaveApi} inválida.", 401);
            }

            var abrangencias = await mediator.Send(new ObterAbrangenciaPorLoginGrupoQuery(autenticacaoDto.Login, autenticacaoDto.Perfil));

            if (abrangencias == null || !abrangencias.Any())
                throw new NaoAutorizadoException($"Usuário: {autenticacaoDto.Login} inválido com o grupo {autenticacaoDto.Perfil}.", 401);

            return await mediator.Send(new GerarCodigoValidacaoAutenticacaoCommand(abrangencias));
        }

    }
}

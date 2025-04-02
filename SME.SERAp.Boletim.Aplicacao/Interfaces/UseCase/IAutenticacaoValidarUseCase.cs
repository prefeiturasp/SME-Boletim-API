using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IAutenticacaoValidarUseCase
    {
        Task<AutenticacaoRetornoDto> Executar(AutenticacaoValidarDto autenticacaoValidarDto);
    }
}

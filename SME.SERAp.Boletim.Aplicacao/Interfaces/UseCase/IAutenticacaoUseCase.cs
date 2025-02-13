using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IAutenticacaoUseCase
    {
        Task<AutenticacaoValidarDto> Executar(AutenticacaoDto autenticacaoDto);
    }
}

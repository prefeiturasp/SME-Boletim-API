using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterUesAbrangenciaUsuarioLogadoUseCase
    {
        Task<IEnumerable<AbrangenciaUeDto>> Executar();
    }
}

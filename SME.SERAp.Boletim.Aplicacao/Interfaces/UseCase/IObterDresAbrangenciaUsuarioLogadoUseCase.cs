using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDresAbrangenciaUsuarioLogadoUseCase
    {
        Task<IEnumerable<DreAbragenciaDetalheDto>> Executar();
    }
}

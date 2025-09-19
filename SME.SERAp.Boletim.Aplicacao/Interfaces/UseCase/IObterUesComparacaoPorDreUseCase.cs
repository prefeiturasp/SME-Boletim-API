using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterUesComparacaoPorDreUseCase
    {
        Task<IEnumerable<UePorDreDto>> Executar(long dreId, int anoAplicacao, int disciplinaId, int anoEscolar);
    }
}

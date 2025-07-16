using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterAnosEscolaresPorLoteIdUseCase
    {
        Task<IEnumerable<AnoEscolarDto>> Executar(long loteId);
    }
}

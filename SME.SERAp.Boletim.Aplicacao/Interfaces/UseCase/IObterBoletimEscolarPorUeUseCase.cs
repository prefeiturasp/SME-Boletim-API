using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarPorUeUseCase
    {
        Task<IEnumerable<BoletimEscolarDto>> Executar(long loteId, long ueId, FiltroBoletimDto filtros);
    }
}
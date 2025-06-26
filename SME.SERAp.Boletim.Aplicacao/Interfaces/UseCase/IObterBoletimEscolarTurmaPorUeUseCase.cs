using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarTurmaPorUeUseCase
    {
        Task<BoletimEscolarPorTurmaDto> Executar(long loteId, long ueId, FiltroBoletimDto filtros);
    }
}

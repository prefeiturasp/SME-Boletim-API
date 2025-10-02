using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterTurmasUeAnoUseCase
    {
        Task<IEnumerable<TurmaAnoDto>> Executar(long loteId, long ueId, int disciplinaId, int anoEscolar);
    }
}

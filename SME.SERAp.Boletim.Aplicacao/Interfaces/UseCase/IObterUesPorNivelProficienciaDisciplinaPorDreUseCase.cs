using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterUesPorNivelProficienciaDisciplinaPorDreUseCase
    {
        Task<DreResumoUesNivelProficienciaDto> Executar(long loteId, long dreId, int anoEscolar);
    }
}

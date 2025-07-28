using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarResumoSmeUseCase
    {
        Task<BoletimEscolarResumoSmeDto> Executar(long loteId, int anoEscolar);
    }
}

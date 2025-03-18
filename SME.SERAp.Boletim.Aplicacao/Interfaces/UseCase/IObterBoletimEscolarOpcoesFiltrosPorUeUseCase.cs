using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarOpcoesFiltrosPorUeUseCase
    {
        Task<BoletimEscolarOpcoesFiltrosDto> Executar(long ueId);
    }
}

using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarTurmaPorUeUseCase
    {
        Task<BoletimEscolarPorTurmaDto> Executar(long ueId);
    }
}

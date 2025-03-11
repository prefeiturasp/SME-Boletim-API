using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarPorUeUseCase
    {
        Task<IEnumerable<BoletimEscolar>> Executar(long ueId);
    }
}
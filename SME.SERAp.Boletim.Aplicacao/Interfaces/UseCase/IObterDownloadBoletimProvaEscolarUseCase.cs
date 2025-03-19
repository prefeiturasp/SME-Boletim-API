using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDownloadBoletimProvaEscolarUseCase
    {
        Task<MemoryStream> Executar(long ueId);
    }
}

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDownloadBoletimProvaEscolarSmeUseCase
    {
        Task<MemoryStream> Executar(long loteId);
    }
}

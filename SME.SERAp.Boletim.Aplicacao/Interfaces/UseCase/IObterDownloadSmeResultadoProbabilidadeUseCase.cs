namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDownloadSmeResultadoProbabilidadeUseCase
    {
        Task<MemoryStream> Executar(long loteId);
    }
}

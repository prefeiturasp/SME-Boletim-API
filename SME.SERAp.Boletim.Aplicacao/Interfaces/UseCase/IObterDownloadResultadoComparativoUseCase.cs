namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDownloadResultadoComparativoUseCase
    {
        Task<MemoryStream> Executar(int ueId, int disciplinaId, int anoEscolar,  long loteId, string? turma,
            List<int>? tiposVariacao, string? nomeAluno);
    }
}

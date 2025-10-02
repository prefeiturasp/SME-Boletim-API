namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterAnosAplicacaoPorDreUseCase
    {
        public Task<IEnumerable<int>> Executar(long dreId);
    }
}

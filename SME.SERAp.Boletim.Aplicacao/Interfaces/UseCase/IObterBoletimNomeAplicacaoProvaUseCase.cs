using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimNomeAplicacaoProvaUseCase
    {
        Task<LoteProvaAtivoDto> Executar();
    }
}

using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterAnosEscolaresPorDreAnoAplicacaoUseCase
    {
        Task<IEnumerable<OpcaoFiltroDto<int>>> Executar(long dreId, int anoAplicacao, int disciplinaId);
    }
}

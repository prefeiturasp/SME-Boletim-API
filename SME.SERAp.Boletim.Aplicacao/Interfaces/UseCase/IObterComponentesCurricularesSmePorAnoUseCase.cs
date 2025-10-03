using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterComponentesCurricularesSmePorAnoUseCase
    {
        Task<IEnumerable<OpcaoFiltroDto<int>>> Executar(int anoAplicacao);
    }
}

using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDresComparativoSmeUseCase
    {
        Task<IEnumerable<DreDto>> Executar(int anoAplicacao, int disciplinaId, int anoEscolar);
    }
}

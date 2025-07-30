using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarDresMediaProficienciaUseCase
    {
        Task<IEnumerable<BoletimEscolarDreMediaProficienciaDto>> Executar(long loteId, int anoEscolar, IEnumerable<long> dresIds);
    }
}

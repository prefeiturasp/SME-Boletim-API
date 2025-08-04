using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterProficienciaDreUseCase
    {
        Task<ProficienciaDreCompletoDto> Executar(int anoEscolar, long loteId, IEnumerable<long> dreIds = null);
    }
}
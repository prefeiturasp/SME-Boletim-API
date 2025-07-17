using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterUesPorDreUseCase
    {
        Task<IEnumerable<UePorDreDto>> Executar(long dreId, int anoEscolar, long loteId);
    }
}
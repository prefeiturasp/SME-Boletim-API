using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterProficienciaComparativoProvaSPUseCase
    {
        Task<ProficienciaUeComparacaoProvaSPDto> Executar(long loteId, int ueId, int disciplinaId, int anoEscolar);
    }
}
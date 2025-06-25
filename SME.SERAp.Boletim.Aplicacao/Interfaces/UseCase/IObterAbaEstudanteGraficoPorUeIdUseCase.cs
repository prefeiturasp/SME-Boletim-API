using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterAbaEstudanteGraficoPorUeIdUseCase
    {
        Task<IEnumerable<AbaEstudanteGraficoDto>> Executar(long loteId, long ueId, FiltroBoletimEstudanteDto filtros);
    }
}
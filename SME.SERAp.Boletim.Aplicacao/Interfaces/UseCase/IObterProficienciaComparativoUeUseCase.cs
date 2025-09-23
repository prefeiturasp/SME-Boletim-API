using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterProficienciaComparativoUeUseCase
    {
        Task<ProficienciaComparativoUeDto> Executar(int dreId, int disciplinaId, int anoLetivo, int anoEscolar, int? ueId, List<int> tiposVariacao, string nomeUe, int? pagina, int? itensPorPagina);
    }
}
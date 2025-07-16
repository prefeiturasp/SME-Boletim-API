using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimEscolarResumoDreUseCase
    {
        Task<BoletimEscolarResumoDreDto> Executar(long loteId, long dreId, int anoEscolar);
    }
}
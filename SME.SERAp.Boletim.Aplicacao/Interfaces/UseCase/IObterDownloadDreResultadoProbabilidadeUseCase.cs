using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterDownloadDreResultadoProbabilidadeUseCase
    {
        Task<MemoryStream> Executar(long loteId, int dreId);
    }
}
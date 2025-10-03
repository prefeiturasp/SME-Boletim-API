using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterAnosAplicacaoPorSmeUseCase
    {
        public Task<IEnumerable<int>> Executar();
    }
}
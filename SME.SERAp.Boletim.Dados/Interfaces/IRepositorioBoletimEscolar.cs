using SME.SERAp.Boletim.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimEscolar : IRepositorioBase<BoletimEscolar>
    {
        Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long ueId);
    }
}
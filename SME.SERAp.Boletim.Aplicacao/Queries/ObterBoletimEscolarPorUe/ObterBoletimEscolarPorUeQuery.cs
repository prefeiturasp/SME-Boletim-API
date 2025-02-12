using MediatR;
using SME.SERAp.Boletim.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe
{
    public class ObterBoletimEscolarPorUeQuery : IRequest<IEnumerable<BoletimEscolar>>
    {
        public ObterBoletimEscolarPorUeQuery(long ueId)
        {
            UeId = ueId;
        }
        public long UeId { get; set; }
    }
}
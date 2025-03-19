using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId
{
    public class ObterAbaEstudanteGraficoPorUeIdQuery : IRequest<IEnumerable<AbaEstudanteGraficoDto>>
    {
        public long UeId { get; set; }

        public ObterAbaEstudanteGraficoPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }
    }
}
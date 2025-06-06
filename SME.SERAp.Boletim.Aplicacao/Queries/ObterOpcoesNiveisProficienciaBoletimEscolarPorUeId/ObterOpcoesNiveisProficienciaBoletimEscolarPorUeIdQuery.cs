﻿using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId
{
    public class ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery : IRequest<IEnumerable<OpcaoFiltroDto<int>>>
    {
        public ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}

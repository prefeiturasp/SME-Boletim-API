﻿using MediatR;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe
{
    public class ObterBoletimEscolarPorUeQuery : IRequest<IEnumerable<BoletimEscolar>>
    {
        public ObterBoletimEscolarPorUeQuery(long loteId, long ueId, FiltroBoletimDto filtros)
        {
            LoteId = loteId;
            UeId = ueId;
            Filtros = filtros;
        }

        public long LoteId { get; set; }

        public long UeId { get; set; }

        public FiltroBoletimDto Filtros { get; set; }
    }
}
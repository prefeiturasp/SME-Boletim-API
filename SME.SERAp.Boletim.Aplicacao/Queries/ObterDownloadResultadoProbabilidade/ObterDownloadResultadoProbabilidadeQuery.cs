﻿using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadResultadoProbabilidade
{
    public class ObterDownloadResultadoProbabilidadeQuery : IRequest<IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        public long UeId { get; }
        public long DisciplinaId { get; }
        public int AnoEscolar { get; }

        public ObterDownloadResultadoProbabilidadeQuery(long ueId, long disciplinaId, int anoEscolar)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }
    }
}
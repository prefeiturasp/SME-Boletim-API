﻿using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterResultadoProbabilidadePorUeUseCase
    {
        Task<ResultadoProbabilidadePaginadoDto> Executar(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
    }
}
﻿using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public static class RegistraMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AlunoMap());
                config.AddMap(new AbrangenciaMap());
                config.AddMap(new BoletimEscolarMap());
                config.AddMap(new BoletimProvaAlunoMap());
                config.AddMap(new BoletimResultadoProbabilidadeMap());
                config.ForDommel();
            });
        }
    }
}
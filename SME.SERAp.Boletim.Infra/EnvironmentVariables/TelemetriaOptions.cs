﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.EnvironmentVariables
{
    public class TelemetriaOptions
    {
        public const string Secao = "Telemetria";
        public bool ApplicationInsights { get; set; }
        public bool Apm { get; set; }
    }
}
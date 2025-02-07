using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.EnvironmentVariables
{
    public class RedisOptions
    {
        public static string Secao => "RedisOptions";
        public string Endpoint { get; set; }
        public Proxy Proxy { get; set; }
        public int SyncTimeout { get; set; } = 5000;
    }
}
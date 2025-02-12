using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Fila
{
    public class ExchangeRabbit
    {
        public static string SerapEstudante => "serap.estudante.workers";
        public static string Serap => "serap.workers";
        public static string SerapEstudanteAcompanhamento => "serap.estudante.acomp.workers";
        public static string SerapEstudanteDeadLetter => "serap.estudante.workers.deadletter";
        public static string Logs => "EnterpriseApplicationLog";
    }
}
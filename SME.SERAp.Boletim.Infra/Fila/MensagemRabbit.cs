using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Fila
{
    public class MensagemRabbit
    {
        public MensagemRabbit(object mensagem, Guid codigoCorrelacao)
        {
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;
        }
        protected MensagemRabbit()
        {

        }

        public object Mensagem { get; set; }
        public Guid CodigoCorrelacao { get; set; }

        public T ObterObjetoMensagem<T>() where T : class
        {
            return Mensagem?.ToString().ConverterObjectStringPraObjeto<T>();
        }
    }
}
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Dominio.Enumerados;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class LogMensagemTest
    {
        [Fact]
        public void Deve_Criar_LogMensagem_Com_Propriedades_Validas()
        {
            var mensagem = "Erro ao acessar serviço";
            var nivel = LogNivel.Critico;
            var observacao = "Falha de autenticação";
            var rastreamento = "Trace123";
            var excecaoInterna = "System.Exception";
            var projeto = "Teste-Projeto";

            var log = new LogMensagem(mensagem, nivel, observacao, rastreamento, excecaoInterna, projeto);

            Assert.Equal(mensagem, log.Mensagem);
            Assert.Equal(nivel, log.Nivel);
            Assert.Equal(observacao, log.Observacao);
            Assert.Equal(rastreamento, log.Rastreamento);
            Assert.Equal(excecaoInterna, log.ExcecaoInterna);
            Assert.Equal(projeto, log.Projeto);
            Assert.True(log.DataHora <= DateTime.Now);
        }
    }
}

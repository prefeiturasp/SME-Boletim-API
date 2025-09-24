using SME.SERAp.Boletim.Infra.Exceptions;
using System.Net;

namespace SME.SERAp.Boletim.Infra.Teste.Exceptions
{
    public class NaoAutorizadoExceptionTeste
    {
        [Fact]
        public void Deve_Criar_Excecao_Com_StatusCode_Padrao()
        {
            var mensagem = "Não autorizado";

            var ex = new NaoAutorizadoException(mensagem);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal(401, ex.StatusCode);
        }

        [Fact]
        public void Deve_Criar_Excecao_Com_StatusCode_Informado()
        {
            var mensagem = "Acesso negado";
            var statusCode = 403;

            var ex = new NaoAutorizadoException(mensagem, statusCode);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Fact]
        public void Deve_Criar_Excecao_Com_HttpStatusCode()
        {
            var mensagem = "Proibido";
            var httpStatus = HttpStatusCode.Forbidden;

            var ex = new NaoAutorizadoException(mensagem, httpStatus);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal((int)httpStatus, ex.StatusCode);
        }
    }
}

using SME.SERAp.Boletim.Infra.Exceptions;
using System.Net;

namespace SME.SERAp.Boletim.Infra.Teste.Exceptions
{
    public class NegocioExceptionTeste
    {
        [Fact]
        public void Deve_Criar_Excecao_Com_StatusCode_Padrao()
        {
            var mensagem = "Conflito de negócio";

            var ex = new NegocioException(mensagem);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal(409, ex.StatusCode);
        }

        [Fact]
        public void Deve_Criar_Excecao_Com_StatusCode_Informado()
        {
            var mensagem = "Erro de negócio";
            var statusCode = 422;

            var ex = new NegocioException(mensagem, statusCode);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Fact]
        public void Deve_Criar_Excecao_Com_HttpStatusCode()
        {
            var mensagem = "Operação não permitida";
            var httpStatus = HttpStatusCode.Forbidden;

            var ex = new NegocioException(mensagem, httpStatus);

            Assert.Equal(mensagem, ex.Message);
            Assert.Equal((int)httpStatus, ex.StatusCode);
        }
    }
}

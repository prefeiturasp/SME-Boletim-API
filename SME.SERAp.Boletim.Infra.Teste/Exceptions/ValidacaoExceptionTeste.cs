using FluentValidation.Results;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Infra.Teste.Exceptions
{
    public class ValidacaoExceptionTeste
    {
        [Fact]
        public void Deve_Criar_Excecao_Com_Erros()
        {
            var erros = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro no campo 1"),
                new ValidationFailure("Campo2", "Erro no campo 2")
            };

            var ex = new ValidacaoException(erros);

            Assert.Equal("Erro no campo 1", ex.Message);
            Assert.Equal(erros, ex.Erros);
            Assert.Equal("Erro no campo 1", ex.Data["Campo1"]);
            Assert.Equal("Erro no campo 2", ex.Data["Campo2"]);
        }

        [Fact]
        public void Mensagens_Deve_Retornar_Lista_De_Erros()
        {
            var erros = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro 1"),
                new ValidationFailure("Campo2", "Erro 2")
            };

            var ex = new ValidacaoException(erros);

            var mensagens = ex.Mensagens().ToList();

            Assert.Equal(2, mensagens.Count);
            Assert.Contains("Erro 1", mensagens);
            Assert.Contains("Erro 2", mensagens);
        }

        [Fact]
        public void Deve_Adicionar_Somente_Uma_Vez_Cada_Erro_No_Data()
        {
            var erros = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro 1"),
                new ValidationFailure("Campo1", "Erro 1")
            };

            var ex = new ValidacaoException(erros);

            Assert.Single(ex.Data);
            Assert.Equal("Erro 1", ex.Data["Campo1"]);
        }
    }
}

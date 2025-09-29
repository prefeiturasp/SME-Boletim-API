using FluentValidation.TestHelper;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidatorTeste
    {
        private readonly ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidator _validator;

        public ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidatorTeste()
        {
            _validator = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryValidator();
        }

        [Fact]
        public void Deve_Falhar_Quando_Codigo_For_Vazio()
        {
            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(string.Empty);

            var resultado = _validator.Validate(query);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Codigo" &&
                                                   e.ErrorMessage == "É necessário informar o código de validação.");
        }

        [Fact]
        public void Deve_Falhar_Quando_Codigo_For_Nulo()
        {
            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(null);

            var resultado = _validator.Validate(query);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Codigo" &&
                                                   e.ErrorMessage == "É necessário informar o código de validação.");
        }

        [Fact]
        public void Deve_Passar_Quando_Codigo_For_Preenchido()
        {
            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery("123456");

            var resultado = _validator.Validate(query);

            Assert.True(resultado.IsValid);
            Assert.Empty(resultado.Errors);
        }
    }
}

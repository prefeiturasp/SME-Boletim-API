using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAbrangenciaPorLoginGrupoQueryValidatorTeste
    {
        private readonly ObterAbrangenciaPorLoginGrupoQueryValidator _validator;

        public ObterAbrangenciaPorLoginGrupoQueryValidatorTeste()
        {
            _validator = new ObterAbrangenciaPorLoginGrupoQueryValidator();
        }

        [Fact]
        public void Deve_Falhar_Quando_Login_For_Vazio()
        {
            var query = new ObterAbrangenciaPorLoginGrupoQuery(string.Empty, Guid.NewGuid());

            var result = _validator.Validate(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Login" &&
                                                e.ErrorMessage == "O login ou código Rf é obrigatório.");
        }

        [Fact]
        public void Deve_Falhar_Quando_Login_For_Nulo()
        {
            var query = new ObterAbrangenciaPorLoginGrupoQuery(null, Guid.NewGuid());

            var result = _validator.Validate(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Login" &&
                                                e.ErrorMessage == "O login ou código Rf é obrigatório.");
        }

        [Fact]
        public void Deve_Falhar_Quando_Perfil_For_Empty()
        {
            var query = new ObterAbrangenciaPorLoginGrupoQuery("LoginValido", Guid.Empty);

            var result = _validator.Validate(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Perfil" &&
                                                e.ErrorMessage == "O id do Grupo é obrigatório.");
        }

        [Fact]
        public void Deve_Passar_Quando_Login_E_Perfil_Forem_Preenchidos()
        {
            var query = new ObterAbrangenciaPorLoginGrupoQuery("LoginValido", Guid.NewGuid());

            var result = _validator.Validate(query);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}

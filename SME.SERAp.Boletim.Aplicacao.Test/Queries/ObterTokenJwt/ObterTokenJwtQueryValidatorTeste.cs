using SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterTokenJwtQueryValidatorTeste
    {
        private readonly ObterTokenJwtQueryValidator _validator;

        public ObterTokenJwtQueryValidatorTeste()
        {
            _validator = new ObterTokenJwtQueryValidator();
        }

        [Fact]
        public void Deve_Falhar_Quando_Abrangencias_For_Nulo()
        {
            var query = new ObterTokenJwtQuery(null);

            var result = _validator.Validate(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Abrangencias" &&
                                                e.ErrorMessage == "Abrangências é obrigatório");
        }

        [Fact]
        public void Deve_Falhar_Quando_Abrangencias_For_Vazio()
        {
            var query = new ObterTokenJwtQuery(new List<AbrangenciaDetalheDto>());

            var result = _validator.Validate(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Abrangencias" &&
                                                e.ErrorMessage == "Abrangências é obrigatório");
        }

        [Fact]
        public void Deve_Passar_Quando_Abrangencias_Tiver_Valores()
        {
            var query = new ObterTokenJwtQuery(new List<AbrangenciaDetalheDto>() { new AbrangenciaDetalheDto("login_teste", Perfis.PERFIL_ADMINISTRADOR) });

            var result = _validator.Validate(query);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}

using SME.SERAp.Boletim.Infra.Cache;

namespace SME.SERAp.Boletim.Infra.Teste.Cache
{
    public class CacheChaveTeste
    {
        [Fact]
        public void ObterChave_Deve_Formatar_Corretamente_Com_Um_Parametro()
        {
            var chave = CacheChave.ObterChave(CacheChave.Autenticacao, 123);

            Assert.Equal("boletim-auth-123", chave);
        }

        [Fact]
        public void ObterChave_Deve_Formatar_Corretamente_Com_Dois_Parametros()
        {
            var chave = CacheChave.ObterChave(CacheChave.AlunoCadernoProva, 456, "RA999");

            Assert.Equal("alcp-456-RA999", chave);
        }

        [Fact]
        public void ObterChave_Deve_Formatar_Corretamente_Exportacao_Resultado()
        {
            var chave = CacheChave.ObterChave(CacheChave.ExportacaoResultado, 10, 999);

            Assert.Equal("exportacao-10-prova-999", chave);
        }

        [Fact]
        public void ObterChave_Deve_Retornar_Constante_Sem_Parametros()
        {
            var chave = CacheChave.ObterChave(CacheChave.Parametros);

            Assert.Equal("parametros", chave);
        }

        [Fact]
        public void ObterChave_Deve_Lancar_Excecao_Quando_Faltam_Parametros()
        {
            Assert.Throws<FormatException>(() =>
                CacheChave.ObterChave(CacheChave.AlunoCadernoProva, 123));
        }

        [Fact]
        public void ObterChave_Deve_Lancar_Excecao_Quando_Nao_Ha_Parametros()
        {
            Assert.Throws<FormatException>(() =>
                CacheChave.ObterChave(CacheChave.Autenticacao));
        }

        [Theory]
        [InlineData(CacheChave.QuestaoCompleta, 77, "qc-77")]
        [InlineData(CacheChave.QuestaoCompletaLegado, 88, "qcl-88")]
        [InlineData(CacheChave.Prova, 55, "p-55")]
        [InlineData(CacheChave.MeusDados, "RA123", "ra-RA123")]
        [InlineData(CacheChave.PreferenciasAluno, "RA321", "prefa-RA321")]
        [InlineData(CacheChave.AlunoDeficiencia, "RA555", "al-def-RA555")]
        public void ObterChave_Deve_Formatar_Corretamente_Varias_Constantes(string template, object parametro, string esperado)
        {
            var chave = CacheChave.ObterChave(template, parametro);

            Assert.Equal(esperado, chave);
        }

        [Fact]
        public void ObterChave_Deve_Formatar_Corretamente_Usuario_Login_Perfil_Dres_Abrangencia()
        {
            var chave = CacheChave.ObterChave(CacheChave.UsuarioLoginPerfilDresAbrangencia, "login123", "perfilABC");

            Assert.Equal("us-login-ue-abrangencia-login123-perfilABC", chave);
        }
    }
}

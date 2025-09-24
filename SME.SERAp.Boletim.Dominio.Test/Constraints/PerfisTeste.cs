using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Dominio.Test.Constraints
{
    public class PerfisTeste
    {
        [Theory]
        [InlineData("AAD9D772-41A3-E411-922D-782BCB3D218E", true)]  // PERFIL_ADMINISTRADOR
        [InlineData("22366A3E-9E4C-E711-9541-782BCB3D218E", true)]  // PERFIL_ADMINISTRADOR_NTA
        [InlineData("E77E81B1-191E-E811-B259-782BCB3D2D76", true)]  // PERFIL_PROFESSOR
        [InlineData("067D9B21-A1FF-E611-9541-782BCB3D218E", true)]  // PERFIL_PROFESSOR_OLD
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76", false)] // PERFIL_DIRETOR_ESCOLAR
        [InlineData("00000000-0000-0000-0000-000000000000", false)] // Guid inexistente
        public void PerfilEhValido_Deve_Retornar_Correto(string guid, bool esperado)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhValido(perfil);

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76")] // Diretor Escolar
        [InlineData("ECF7A20D-1A1E-E811-B259-782BCB3D2D76")] // Assistente Diretor
        [InlineData("D4026F2C-1A1E-E811-B259-782BCB3D2D76")] // Coordenador
        [InlineData("00000000-0000-0000-0000-000000000000")] // Guid inexistente
        public void PerfilEhValido_Deve_Retornar_False_Para_Perfis_Invalidos(string guid)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhValido(perfil);

            Assert.False(resultado);
        }

        [Theory]
        [InlineData("AAD9D772-41A3-E411-922D-782BCB3D218E", true)]  // Administrador
        [InlineData("22366A3E-9E4C-E711-9541-782BCB3D218E", true)]  // Administrador NTA
        [InlineData("E77E81B1-191E-E811-B259-782BCB3D2D76", false)] // Professor
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76", false)] // Diretor
        public void PerfilEhAdministrador_Deve_Retornar_Correto(string guid, bool esperado)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhAdministrador(perfil);

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("E77E81B1-191E-E811-B259-782BCB3D2D76")] // Professor
        [InlineData("067D9B21-A1FF-E611-9541-782BCB3D218E")] // Professor Old
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76")] // Diretor
        [InlineData("D4026F2C-1A1E-E811-B259-782BCB3D2D76")] // Coordenador
        public void PerfilEhAdministrador_Deve_Retornar_False_Para_Nao_Administradores(string guid)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhAdministrador(perfil);

            Assert.False(resultado);
        }

        [Theory]
        [InlineData("E77E81B1-191E-E811-B259-782BCB3D2D76", true)]  // Professor
        [InlineData("067D9B21-A1FF-E611-9541-782BCB3D218E", true)]  // Professor Old
        [InlineData("AAD9D772-41A3-E411-922D-782BCB3D218E", false)] // Administrador
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76", false)] // Diretor
        public void PerfilEhProfessor_Deve_Retornar_Correto(string guid, bool esperado)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhProfessor(perfil);

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("AAD9D772-41A3-E411-922D-782BCB3D218E")] // Administrador
        [InlineData("22366A3E-9E4C-E711-9541-782BCB3D218E")] // Administrador NTA
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76")] // Diretor
        [InlineData("D4026F2C-1A1E-E811-B259-782BCB3D2D76")] // Coordenador
        public void PerfilEhProfessor_Deve_Retornar_False_Para_Nao_Professores(string guid)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.PerfilEhProfessor(perfil);

            Assert.False(resultado);
        }

        [Theory]
        [InlineData("AAD9D772-41A3-E411-922D-782BCB3D218E", TipoPerfil.Administrador)]        // Administrador
        [InlineData("22366A3E-9E4C-E711-9541-782BCB3D218E", TipoPerfil.Administrador)]        // Administrador NTA
        [InlineData("A8CB8D7B-F333-E711-9541-782BCB3D218E", TipoPerfil.Administrador)]        // Administrador COPED LEITURA
        [InlineData("104F0759-87E8-E611-9541-782BCB3D218E", TipoPerfil.Administrador_DRE)]    // Administrador DRE
        [InlineData("4318D329-17DC-4C48-8E59-7D80557F7E77", TipoPerfil.Administrador_DRE)]    // Administrador NA DRE
        [InlineData("75DCAB30-2C1E-E811-B259-782BCB3D2D76", TipoPerfil.Diretor)]              // Diretor Escolar
        [InlineData("ECF7A20D-1A1E-E811-B259-782BCB3D2D76", TipoPerfil.Diretor)]              // Assistente Diretor UE
        [InlineData("D4026F2C-1A1E-E811-B259-782BCB3D2D76", TipoPerfil.Coordenador)]          // Coordenador Pedagógico
        [InlineData("E77E81B1-191E-E811-B259-782BCB3D2D76", TipoPerfil.Professor)]            // Professor
        [InlineData("067D9B21-A1FF-E611-9541-782BCB3D218E", TipoPerfil.Professor)]            // Professor Old
        public void ObterTipoPerfil_Deve_Retornar_Correto(string guid, TipoPerfil esperado)
        {
            var perfil = Guid.Parse(guid);

            var resultado = Perfis.ObterTipoPerfil(perfil);

            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void ObterTipoPerfil_Deve_Retornar_Professor_Quando_Perfil_Invalido()
        {
            var perfilInvalido = Guid.NewGuid();

            var resultado = Perfis.ObterTipoPerfil(perfilInvalido);

            Assert.Equal(TipoPerfil.Professor, resultado);
        }

        [Theory]
        [InlineData(TipoPerfil.Administrador, true)]
        [InlineData(TipoPerfil.Administrador_DRE, true)]
        [InlineData(TipoPerfil.Diretor, false)]
        [InlineData(TipoPerfil.Coordenador, false)]
        [InlineData(TipoPerfil.Professor, false)]
        public void PodeVisualizarDre_Deve_Retornar_Correto(TipoPerfil tipoPerfil, bool esperado)
        {
            var resultado = Perfis.PodeVisualizarDre(tipoPerfil);

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData(TipoPerfil.Diretor)]
        [InlineData(TipoPerfil.Coordenador)]
        [InlineData(TipoPerfil.Professor)]
        public void PodeVisualizarDre_Deve_Retornar_False_Para_Perfis_Nao_Autorizados(TipoPerfil tipoPerfil)
        {
            var resultado = Perfis.PodeVisualizarDre(tipoPerfil);

            Assert.False(resultado);
        }
    }
}

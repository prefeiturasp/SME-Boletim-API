using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Dominio.Test.Enumerados
{
    public class TipoPerfilTest
    {
        [Fact]
        public void Deve_Conter_Valores_Corretos_No_Enum_TipoPerfil()
        {
            Assert.Equal(1, (int)TipoPerfil.Professor);
            Assert.Equal(2, (int)TipoPerfil.Coordenador);
            Assert.Equal(3, (int)TipoPerfil.Diretor);
            Assert.Equal(4, (int)TipoPerfil.Administrador_DRE);
            Assert.Equal(5, (int)TipoPerfil.Administrador);
        }

        [Fact]
        public void Deve_Converter_De_Int_Para_TipoPerfil_Corretamente()
        {
            var tipoPerfil = (TipoPerfil)3;
            Assert.Equal(TipoPerfil.Diretor, tipoPerfil);
        }

        [Fact]
        public void Deve_Converter_De_TipoPerfil_Para_String()
        {
            var tipoPerfil = TipoPerfil.Diretor;
            Assert.Equal("Diretor", tipoPerfil.ToString());
        }
    }
}

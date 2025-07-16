using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class EntidadeBaseTest
    {
        private class EntidadeFake : EntidadeBase { }

        [Fact]
        public void Deve_Criar_EntidadeBase_Com_Id()
        {
            var entidade = new EntidadeFake
            {
                Id = 99
            };

            Assert.Equal(99, entidade.Id);
        }
    }
}

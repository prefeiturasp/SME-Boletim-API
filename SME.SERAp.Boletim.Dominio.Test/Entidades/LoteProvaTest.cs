using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;
using System;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class LoteProvaTest
    {
        [Fact]
        public void Deve_Criar_LoteProva_Com_Propriedades_Validas()
        {
            var lote = new LoteProva
            {
                Id = 1,
                Nome = "Lote Exemplo",
                TipoTai = true,
                ExibirNoBoletim = true,
                DataCorrecaoFim = new DateTime(2025, 6, 10),
                DataInicioLote = new DateTime(2025, 6, 1),
                DataCriacao = new DateTime(2025, 5, 20),
                DataAlteracao = new DateTime(2025, 6, 5)
            };

            Assert.Equal(1, lote.Id);
            Assert.Equal("Lote Exemplo", lote.Nome);
            Assert.True(lote.TipoTai);
            Assert.True(lote.ExibirNoBoletim);
            Assert.Equal(new DateTime(2025, 6, 10), lote.DataCorrecaoFim);
            Assert.Equal(new DateTime(2025, 6, 1), lote.DataInicioLote);
            Assert.Equal(new DateTime(2025, 5, 20), lote.DataCriacao);
            Assert.Equal(new DateTime(2025, 6, 5), lote.DataAlteracao);
        }
    }
}

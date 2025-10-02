using Xunit;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.Abrangencia
{
    public class DreAbragenciaDetalheDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var id = 1L;
            var codigo = "CODIGO01";
            var abreviacao = "DRE-JA";
            var nome = "Diretoria Regional de Educação Jacanã";

            var dto = new DreAbragenciaDetalheDto
            {
                Id = id,
                Codigo = codigo,
                Abreviacao = abreviacao,
                Nome = nome
            };

            Assert.Equal(id, dto.Id);
            Assert.Equal(codigo, dto.Codigo);
            Assert.Equal(abreviacao, dto.Abreviacao);
            Assert.Equal(nome, dto.Nome);
        }
    }
}
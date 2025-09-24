using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos
{
    public class PaginacaoDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var itens = new List<string> { "A", "B", "C" };
            var pagina = 1;
            var tamanhoPagina = 2;
            var totalRegistros = 3;

            var dto = new PaginacaoDto<string>(itens, pagina, tamanhoPagina, totalRegistros);

            Assert.Equal(itens, dto.Itens);
            Assert.Equal(pagina, dto.Pagina);
            Assert.Equal(tamanhoPagina, dto.TamanhoPagina);
            Assert.Equal(totalRegistros, dto.TotalRegistros);
            Assert.Equal(2, dto.TotalPaginas);
        }

        [Fact]
        public void Deve_Calcular_TotalPaginas_Corretamente()
        {
            var itens = new List<int> { 1, 2, 3, 4, 5 };
            var pagina = 2;
            var tamanhoPagina = 2;
            var totalRegistros = 5;

            var dto = new PaginacaoDto<int>(itens, pagina, tamanhoPagina, totalRegistros);

            Assert.Equal(3, dto.TotalPaginas);
        }

        [Fact]
        public void Deve_Retornar_TotalPaginas_Um_Quando_TotalRegistros_Menor_Que_TamanhoPagina()
        {
            var itens = new List<string> { "X" };
            var pagina = 1;
            var tamanhoPagina = 10;
            var totalRegistros = 5;

            var dto = new PaginacaoDto<string>(itens, pagina, tamanhoPagina, totalRegistros);

            Assert.Equal(1, dto.TotalPaginas);
        }

        [Fact]
        public void Deve_Retornar_TotalPaginas_Zero_Quando_TotalRegistros_For_Zero()
        {
            var itens = new List<string>();
            var pagina = 1;
            var tamanhoPagina = 10;
            var totalRegistros = 0;

            var dto = new PaginacaoDto<string>(itens, pagina, tamanhoPagina, totalRegistros);

            Assert.Equal(0, dto.TotalPaginas);
        }
    }
}

using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class PaginacaoUesBoletimDadosDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var itens = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto { Disciplinas = new List<UeBoletimDisciplinaProficienciaDto> { new UeBoletimDisciplinaProficienciaDto(), new UeBoletimDisciplinaProficienciaDto() } },
                new UeDadosBoletimDto { Disciplinas = new List<UeBoletimDisciplinaProficienciaDto> { new UeBoletimDisciplinaProficienciaDto() } }
            };
            var pagina = 1;
            var tamanhoPagina = 10;
            var totalRegistros = 2;

            var dto = new PaginacaoUesBoletimDadosDto(itens, pagina, tamanhoPagina, totalRegistros);

            Assert.Equal(pagina, dto.Pagina);
            Assert.Equal(tamanhoPagina, dto.TamanhoPagina);
            Assert.Equal(totalRegistros, dto.TotalRegistros);
            Assert.Equal(itens, dto.Itens);
        }

        [Fact]
        public void Deve_Calcular_TotalTipoDisciplina_Corretamente()
        {
            var itens = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto { Disciplinas = new List<UeBoletimDisciplinaProficienciaDto> { new UeBoletimDisciplinaProficienciaDto(), new UeBoletimDisciplinaProficienciaDto(), new UeBoletimDisciplinaProficienciaDto() } },
                new UeDadosBoletimDto { Disciplinas = new List<UeBoletimDisciplinaProficienciaDto> { new UeBoletimDisciplinaProficienciaDto(), new UeBoletimDisciplinaProficienciaDto() } }
            };

            var dto = new PaginacaoUesBoletimDadosDto(itens, 1, 10, 2);

            Assert.Equal(3, dto.TotalTipoDisciplina);
        }

        [Fact]
        public void Deve_Retornar_Zero_Quando_Lista_Vazia()
        {
            var itens = new List<UeDadosBoletimDto>();

            var dto = new PaginacaoUesBoletimDadosDto(itens, 1, 10, 0);

            Assert.Equal(0, dto.TotalTipoDisciplina);
        }

        [Fact]
        public void Deve_Retornar_Zero_Quando_Disciplinas_Forem_Nulas()
        {
            var itens = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto { Disciplinas = null }
            };

            var dto = new PaginacaoUesBoletimDadosDto(itens, 1, 10, 1);

            Assert.Equal(0, dto.TotalTipoDisciplina);
        }
    }
}

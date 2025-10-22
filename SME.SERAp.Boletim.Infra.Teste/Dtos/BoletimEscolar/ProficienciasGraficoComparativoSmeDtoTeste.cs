using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class ProficienciasGraficoComparativoSmeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_DreAbreviacao_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreAbreviacao = "DRE-CS"
            };

            Assert.Equal("DRE-CS", dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Atribuir_DreNome_Corretamente()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreNome = "Diretoria Regional de Educação Campo Limpo"
            };

            Assert.Equal("Diretoria Regional de Educação Campo Limpo", dto.DreNome);
        }

        [Fact]
        public void Deve_Atribuir_ListaProficienciaGraficoComparativoDto_Corretamente()
        {
            var lista = new List<ProficienciasGraficoComparativoDreDto>
            {
                new ProficienciasGraficoComparativoDreDto(),
                new ProficienciasGraficoComparativoDreDto()
            };

            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                ListaProficienciaGraficoComparativoDto = lista
            };

            Assert.Equal(lista, dto.ListaProficienciaGraficoComparativoDto);
            Assert.Equal(2, dto.ListaProficienciaGraficoComparativoDto.Count());
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var lista = new List<ProficienciasGraficoComparativoDreDto>
            {
                new ProficienciasGraficoComparativoDreDto(),
                new ProficienciasGraficoComparativoDreDto(),
                new ProficienciasGraficoComparativoDreDto()
            };

            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreAbreviacao = "DRE-BT",
                DreNome = "Diretoria Regional de Educação Butantã",
                ListaProficienciaGraficoComparativoDto = lista
            };

            Assert.Equal("DRE-BT", dto.DreAbreviacao);
            Assert.Equal("Diretoria Regional de Educação Butantã", dto.DreNome);
            Assert.Equal(lista, dto.ListaProficienciaGraficoComparativoDto);
            Assert.Equal(3, dto.ListaProficienciaGraficoComparativoDto.Count());
        }

        [Fact]
        public void Deve_Permitir_DreAbreviacao_Nula()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreAbreviacao = null
            };

            Assert.Null(dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Permitir_DreNome_Nulo()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreNome = null
            };

            Assert.Null(dto.DreNome);
        }

        [Fact]
        public void Deve_Permitir_DreAbreviacao_Vazia()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreAbreviacao = string.Empty
            };

            Assert.Equal(string.Empty, dto.DreAbreviacao);
        }

        [Fact]
        public void Deve_Permitir_DreNome_Vazio()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                DreNome = string.Empty
            };

            Assert.Equal(string.Empty, dto.DreNome);
        }

        [Fact]
        public void Deve_Permitir_ListaProficienciaGraficoComparativoDto_Nula()
        {
            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                ListaProficienciaGraficoComparativoDto = null
            };

            Assert.Null(dto.ListaProficienciaGraficoComparativoDto);
        }

        [Fact]
        public void Deve_Permitir_ListaProficienciaGraficoComparativoDto_Vazia()
        {
            var lista = new List<ProficienciasGraficoComparativoDreDto>();

            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                ListaProficienciaGraficoComparativoDto = lista
            };

            Assert.NotNull(dto.ListaProficienciaGraficoComparativoDto);
            Assert.Empty(dto.ListaProficienciaGraficoComparativoDto);
        }

        [Fact]
        public void Deve_Permitir_ListaProficienciaGraficoComparativoDto_Com_Um_Item()
        {
            var lista = new List<ProficienciasGraficoComparativoDreDto>
            {
                new ProficienciasGraficoComparativoDreDto()
            };

            var dto = new ProficienciasGraficoComparativoSmeDto
            {
                ListaProficienciaGraficoComparativoDto = lista
            };

            Assert.Single(dto.ListaProficienciaGraficoComparativoDto);
        }
    }
}

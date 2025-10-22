using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class GraficoComparativoSmeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_TodasAplicacoesDisponiveis_Corretamente()
        {
            var aplicacoes = new List<string> { "Matemática", "Português", "Ciências" };

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes
            };

            Assert.Equal(aplicacoes, dto.TodasAplicacoesDisponiveis);
            Assert.Equal(3, dto.TodasAplicacoesDisponiveis.Count());
        }

        [Fact]
        public void Deve_Atribuir_Dados_Corretamente()
        {
            var dados = new List<ProficienciasGraficoComparativoSmeDto>
            {
                new ProficienciasGraficoComparativoSmeDto(),
                new ProficienciasGraficoComparativoSmeDto()
            };

            var dto = new GraficoComparativoSmeDto
            {
                Dados = dados
            };

            Assert.Equal(dados, dto.Dados);
            Assert.Equal(2, dto.Dados.Count());
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var aplicacoes = new List<string> { "Matemática", "Português" };
            var dados = new List<ProficienciasGraficoComparativoSmeDto>
            {
                new ProficienciasGraficoComparativoSmeDto(),
                new ProficienciasGraficoComparativoSmeDto(),
                new ProficienciasGraficoComparativoSmeDto()
            };

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes,
                Dados = dados
            };

            Assert.Equal(aplicacoes, dto.TodasAplicacoesDisponiveis);
            Assert.Equal(2, dto.TodasAplicacoesDisponiveis.Count());
            Assert.Equal(dados, dto.Dados);
            Assert.Equal(3, dto.Dados.Count());
        }

        [Fact]
        public void Deve_Permitir_TodasAplicacoesDisponiveis_Nula()
        {
            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = null
            };

            Assert.Null(dto.TodasAplicacoesDisponiveis);
        }

        [Fact]
        public void Deve_Permitir_Dados_Nulo()
        {
            var dto = new GraficoComparativoSmeDto
            {
                Dados = null
            };

            Assert.Null(dto.Dados);
        }

        [Fact]
        public void Deve_Permitir_TodasAplicacoesDisponiveis_Vazia()
        {
            var aplicacoes = new List<string>();

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes
            };

            Assert.NotNull(dto.TodasAplicacoesDisponiveis);
            Assert.Empty(dto.TodasAplicacoesDisponiveis);
        }

        [Fact]
        public void Deve_Permitir_Dados_Vazio()
        {
            var dados = new List<ProficienciasGraficoComparativoSmeDto>();

            var dto = new GraficoComparativoSmeDto
            {
                Dados = dados
            };

            Assert.NotNull(dto.Dados);
            Assert.Empty(dto.Dados);
        }

        [Fact]
        public void Deve_Permitir_TodasAplicacoesDisponiveis_Com_Um_Item()
        {
            var aplicacoes = new List<string> { "Matemática" };

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes
            };

            Assert.Single(dto.TodasAplicacoesDisponiveis);
        }

        [Fact]
        public void Deve_Permitir_Dados_Com_Um_Item()
        {
            var dados = new List<ProficienciasGraficoComparativoSmeDto>
            {
                new ProficienciasGraficoComparativoSmeDto()
            };

            var dto = new GraficoComparativoSmeDto
            {
                Dados = dados
            };

            Assert.Single(dto.Dados);
        }

        [Fact]
        public void Deve_Permitir_TodasAplicacoesDisponiveis_Com_Strings_Vazias()
        {
            var aplicacoes = new List<string> { string.Empty, "", "Matemática" };

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes
            };

            Assert.Equal(3, dto.TodasAplicacoesDisponiveis.Count());
            Assert.Equal(string.Empty, dto.TodasAplicacoesDisponiveis.First());
        }

        [Fact]
        public void Deve_Permitir_TodasAplicacoesDisponiveis_Com_Valores_Nulos()
        {
            var aplicacoes = new List<string> { null, "Matemática", null };

            var dto = new GraficoComparativoSmeDto
            {
                TodasAplicacoesDisponiveis = aplicacoes
            };

            Assert.Equal(3, dto.TodasAplicacoesDisponiveis.Count());
            Assert.Null(dto.TodasAplicacoesDisponiveis.First());
        }
    }
}

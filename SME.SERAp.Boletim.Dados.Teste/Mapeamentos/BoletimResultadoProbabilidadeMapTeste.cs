using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class BoletimResultadoProbabilidadeMapTeste
    {
        static BoletimResultadoProbabilidadeMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BoletimResultadoProbabilidadeMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (BoletimResultadoProbabilidadeMap)FluentMapper.EntityMaps[typeof(BoletimResultadoProbabilidade)];

            Assert.Equal("boletim_resultado_probabilidade", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (BoletimResultadoProbabilidadeMap)FluentMapper.EntityMaps[typeof(BoletimResultadoProbabilidade)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.Id)).ColumnName);
            Assert.Equal("habilidade_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.HabilidadeId)).ColumnName);
            Assert.Equal("codigo_habilidade", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.CodigoHabilidade)).ColumnName);
            Assert.Equal("habilidade_descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.HabilidadeDescricao)).ColumnName);
            Assert.Equal("turma_descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.TurmaDescricao)).ColumnName);
            Assert.Equal("turma_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.TurmaId)).ColumnName);
            Assert.Equal("prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.ProvaId)).ColumnName);
            Assert.Equal("ue_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.UeId)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.DisciplinaId)).ColumnName);
            Assert.Equal("ano_escolar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.AnoEscolar)).ColumnName);
            Assert.Equal("abaixo_do_basico", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.AbaixoDoBasico)).ColumnName);
            Assert.Equal("basico", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.Basico)).ColumnName);
            Assert.Equal("adequado", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.Adequado)).ColumnName);
            Assert.Equal("avancado", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.Avancado)).ColumnName);
            Assert.Equal("data_consolidacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.DataConsolidacao)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (BoletimResultadoProbabilidadeMap)FluentMapper.EntityMaps[typeof(BoletimResultadoProbabilidade)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimResultadoProbabilidade.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

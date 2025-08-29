using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class AbrangenciaMapTeste
    {
        static AbrangenciaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AbrangenciaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (AbrangenciaMap)FluentMapper.EntityMaps[typeof(Abrangencia)];

            Assert.Equal("abrangencia", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (AbrangenciaMap)FluentMapper.EntityMaps[typeof(Abrangencia)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.Id)).ColumnName);
            Assert.Equal("usuario_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.UsuarioId)).ColumnName);
            Assert.Equal("grupo_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.GrupoId)).ColumnName);
            Assert.Equal("dre_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.DreId)).ColumnName);
            Assert.Equal("ue_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.UeId)).ColumnName);
            Assert.Equal("turma_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.TurmaId)).ColumnName);
            Assert.Equal("inicio", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.Inicio)).ColumnName);
            Assert.Equal("fim", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.Fim)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (AbrangenciaMap)FluentMapper.EntityMaps[typeof(Abrangencia)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Abrangencia.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

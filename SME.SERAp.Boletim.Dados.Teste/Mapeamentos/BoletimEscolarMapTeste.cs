using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class BoletimEscolarMapTeste
    {
        static BoletimEscolarMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BoletimEscolarMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (BoletimEscolarMap)FluentMapper.EntityMaps[typeof(BoletimEscolar)];

            Assert.Equal("boletim_escolar", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (BoletimEscolarMap)FluentMapper.EntityMaps[typeof(BoletimEscolar)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Id)).ColumnName);
            Assert.Equal("ue_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.UeId)).ColumnName);
            Assert.Equal("prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.ProvaId)).ColumnName);
            Assert.Equal("componente_curricular", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.ComponenteCurricular)).ColumnName);
            Assert.Equal("abaixo_basico", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.AbaixoBasico)).ColumnName);
            Assert.Equal("abaixo_basico_porcentagem", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.AbaixoBasicoPorcentagem)).ColumnName);
            Assert.Equal("basico", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Basico)).ColumnName);
            Assert.Equal("basico_porcentagem", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.BasicoPorcentagem)).ColumnName);
            Assert.Equal("adequado", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Adequado)).ColumnName);
            Assert.Equal("adequado_porcentagem", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.AdequadoPorcentagem)).ColumnName);
            Assert.Equal("avancado", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Avancado)).ColumnName);
            Assert.Equal("avancado_porcentagem", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.AvancadoPorcentagem)).ColumnName);
            Assert.Equal("total", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Total)).ColumnName);
            Assert.Equal("media_proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.MediaProficiencia)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (BoletimEscolarMap)FluentMapper.EntityMaps[typeof(BoletimEscolar)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimEscolar.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

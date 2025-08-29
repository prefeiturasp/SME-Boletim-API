using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class LoteProvaMapTeste
    {
        static LoteProvaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new LoteProvaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (LoteProvaMap)FluentMapper.EntityMaps[typeof(LoteProva)];

            Assert.Equal("lote_prova", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (LoteProvaMap)FluentMapper.EntityMaps[typeof(LoteProva)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.Id)).ColumnName);
            Assert.Equal("nome", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.Nome)).ColumnName);
            Assert.Equal("tipo_tai", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.TipoTai)).ColumnName);
            Assert.Equal("exibir_no_boletim", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.ExibirNoBoletim)).ColumnName);
            Assert.Equal("data_correcao_fim", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.DataCorrecaoFim)).ColumnName);
            Assert.Equal("data_inicio_lote", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.DataInicioLote)).ColumnName);
            Assert.Equal("data_criacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.DataCriacao)).ColumnName);
            Assert.Equal("data_alteracao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.DataAlteracao)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (LoteProvaMap)FluentMapper.EntityMaps[typeof(LoteProva)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(LoteProva.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

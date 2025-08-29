using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class AlunoMapTeste
    {
        static AlunoMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AlunoMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (AlunoMap)FluentMapper.EntityMaps[typeof(Aluno)];

            Assert.Equal("aluno", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (AlunoMap)FluentMapper.EntityMaps[typeof(Aluno)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.Id)).ColumnName);
            Assert.Equal("nome", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.Nome)).ColumnName);
            Assert.Equal("ra", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.RA)).ColumnName);
            Assert.Equal("situacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.Situacao)).ColumnName);
            Assert.Equal("data_nascimento", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.DataNascimento)).ColumnName);
            Assert.Equal("sexo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.Sexo)).ColumnName);
            Assert.Equal("nome_social", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.NomeSocial)).ColumnName);
            Assert.Equal("turma_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.TurmaId)).ColumnName);
            Assert.Equal("data_atualizacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.DataAtualizacao)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (AlunoMap)FluentMapper.EntityMaps[typeof(Aluno)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Aluno.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

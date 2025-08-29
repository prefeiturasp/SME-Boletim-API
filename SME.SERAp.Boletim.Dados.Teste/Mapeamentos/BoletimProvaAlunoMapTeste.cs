using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    public class BoletimProvaAlunoMapTeste
    {
        static BoletimProvaAlunoMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BoletimProvaAlunoMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (BoletimProvaAlunoMap)FluentMapper.EntityMaps[typeof(BoletimProvaAluno)];

            Assert.Equal("boletim_prova_aluno", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (BoletimProvaAlunoMap)FluentMapper.EntityMaps[typeof(BoletimProvaAluno)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.Id)).ColumnName);
            Assert.Equal("dre_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.DreId)).ColumnName);
            Assert.Equal("ue_codigo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.CodigoUe)).ColumnName);
            Assert.Equal("ue_nome", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.NomeUe)).ColumnName);
            Assert.Equal("prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.ProvaId)).ColumnName);
            Assert.Equal("prova_descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.ProvaDescricao)).ColumnName);
            Assert.Equal("ano_escolar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.AnoEscolar)).ColumnName);
            Assert.Equal("turma", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.Turma)).ColumnName);
            Assert.Equal("aluno_ra", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.AlunoRa)).ColumnName);
            Assert.Equal("aluno_nome", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.AlunoNome)).ColumnName);
            Assert.Equal("disciplina", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.Disciplina)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.DisciplinaId)).ColumnName);
            Assert.Equal("status_prova", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.ProvaStatus)).ColumnName);
            Assert.Equal("proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.Proficiencia)).ColumnName);
            Assert.Equal("erro_medida", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.ErroMedida)).ColumnName);
            Assert.Equal("nivel_codigo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.NivelCodigo)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (BoletimProvaAlunoMap)FluentMapper.EntityMaps[typeof(BoletimProvaAluno)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimProvaAluno.Id)).ColumnName;

            Assert.Equal("id", idColumn);
        }
    }
}

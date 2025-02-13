using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class AlunoMap : DommelEntityMap<Aluno>
    {
        public AlunoMap()
        {
            ToTable("aluno");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.RA).ToColumn("ra");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.DataNascimento).ToColumn("data_nascimento");
            Map(c => c.Sexo).ToColumn("sexo");
            Map(c => c.NomeSocial).ToColumn("nome_social");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
        }
    }
}
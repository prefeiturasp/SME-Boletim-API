using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class BoletimResultadoProbabilidadeMap : DommelEntityMap<BoletimResultadoProbabilidade>
    {
        public BoletimResultadoProbabilidadeMap()
        {
            ToTable("boletim_resultado_probabilidade");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.HabilidadeId).ToColumn("habilidade_id");
            Map(c => c.CodigoHabilidade).ToColumn("codigo_habilidade");
            Map(c => c.HabilidadeDescricao).ToColumn("habilidade_descricao");
            Map(c => c.TurmaDescricao).ToColumn("turma_descricao");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.AnoEscolar).ToColumn("ano_escolar");
            Map(c => c.AbaixoDoBasico).ToColumn("abaixo_do_basico");
            Map(c => c.Basico).ToColumn("basico");
            Map(c => c.Adequado).ToColumn("adequado");
            Map(c => c.Avancado).ToColumn("avancado");
            Map(c => c.DataConsolidacao).ToColumn("data_consolidacao");
        }
    }
}
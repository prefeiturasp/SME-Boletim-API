using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class LoteProvaMap : DommelEntityMap<LoteProva>
    {
        public LoteProvaMap()
        {
            ToTable("lote_prova");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TipoTai).ToColumn("tipo_tai");
            Map(c => c.ExibirNoBoletim).ToColumn("exibir_no_boletim");
            Map(c => c.DataCorrecaoFim).ToColumn("data_correcao_fim");
            Map(c => c.DataInicioLote).ToColumn("data_inicio_lote");
            Map(c => c.DataCriacao).ToColumn("data_criacao");
            Map(c => c.DataAlteracao).ToColumn("data_alteracao");
        }
    }
}

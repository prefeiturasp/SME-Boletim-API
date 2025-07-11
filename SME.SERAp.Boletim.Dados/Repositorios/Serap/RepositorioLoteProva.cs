using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioLoteProva : RepositorioBase<LoteProva>, IRepositorioLoteProva
    {
        public RepositorioLoteProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<LoteProvaDto>> ObterLotesProva()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            lp.id,
	                            lp.nome,
	                            lp.tipo_tai as tipoTai,
                                lp.exibir_no_boletim as exibirNoBoletim,
	                            lp.data_inicio_lote as dataInicioLote
                            from 
	                            lote_prova lp
                            order by lp.id desc";

                return await conn.QueryAsync<LoteProvaDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AnoEscolarDto>> ObterAnosEscolaresPorLoteId(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
	                                        pa.ano,
	                                        pa.modalidade
                                        from
	                                        lote_prova lp
                                        inner join boletim_lote_prova blp on
	                                        blp.lote_id = lp.id
                                        inner join prova_ano_original pa on
	                                        pa.prova_id = blp.prova_id
                                        where
	                                        lp.id = @loteId
                                        group by
	                                        pa.ano,
	                                        pa.modalidade
                                        order by
	                                        pa.ano";

                return await conn.QueryAsync<AnoEscolarDto>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}

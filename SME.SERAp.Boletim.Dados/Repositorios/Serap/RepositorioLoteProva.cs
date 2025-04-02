using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioLoteProva : RepositorioBase<LoteProva>, IRepositorioLoteProva
    {
        public RepositorioLoteProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<LoteProvaAtivoDto> ObterLoteProvaAtivo()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            lp.id,
	                            lp.nome,
	                            lp.tipo_tai as tipoTai,
	                            lp.data_inicio_lote as dataInicioLote
                            from 
	                            lote_prova lp 
                            where 
	                            lp.exibir_no_boletim
                            limit 1;";

                return await conn.QueryFirstAsync<LoteProvaAtivoDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}

using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioAbrangencia : RepositorioBase<Abrangencia>, IRepositorioAbrangencia
    {
        public RepositorioAbrangencia(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<AbrangenciaDetalheDto>> ObterAbrangenciaPorLoginGrupo(string login, long grupoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT a.id as Id,
                                a.usuario_id as UsuarioId,
                                usc.login as Login,
                                usc.nome as Usuario,
                                a.grupo_id as GrupoId,
                                gsc.id_coresso as IdCoreSSO,
                                gsc.nome as Grupo,
                                a.dre_id as DreId,
                                a.ue_id as UeId,
                                a.turma_id as TurmaId,
                                a.inicio as Inicio,
                                a.fim as Fim
                            FROM abrangencia a
                            LEFT JOIN usuario_serap_coresso usc ON usc.id = a.usuario_id
                            LEFT JOIN grupo_serap_coresso gsc ON gsc.id = a.grupo_id
                            WHERE usc.login = @login AND a.grupo_id = @grupoId";

                var resultado = await conn.QueryAsync<AbrangenciaDetalheDto>(query, new { login, grupoId });
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}

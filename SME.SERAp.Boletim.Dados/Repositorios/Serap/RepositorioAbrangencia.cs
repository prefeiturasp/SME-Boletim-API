using System.Linq;
using Dapper;
using DnsClient;
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

        public async Task<IEnumerable<AbrangenciaDetalheDto>> ObterAbrangenciaPorLogin(string login)
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
                            WHERE usc.login = @login";

                var resultado = await conn.QueryAsync<AbrangenciaDetalheDto>(query, new { login });
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AbrangenciaDetalheDto>> ObterAbrangenciaPorLoginGrupo(string login, Guid perfil)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT a.id as Id,
                                a.usuario_id as UsuarioId,
                                usc.login as Login,
                                usc.nome as Usuario,
                                a.grupo_id as GrupoId,
                                gsc.id_coresso as perfil,
                                gsc.nome as Grupo,
                                a.dre_id as DreId,
                                a.ue_id as UeId,
                                a.turma_id as TurmaId,
                                a.inicio as Inicio,
                                a.fim as Fim
                            FROM abrangencia a
                            INNER JOIN usuario_serap_coresso usc ON usc.id = a.usuario_id
                            INNER JOIN grupo_serap_coresso gsc ON gsc.id = a.grupo_id
                            WHERE usc.login = @login AND gsc.id_coresso = @perfil";

                var resultado = await conn.QueryAsync<AbrangenciaDetalheDto>(query, new { login, perfil });
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AbrangenciaUeDto>> ObterUesPorAbrangenciaDre(long dreId, long? ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT
                                d.id AS DreId,
                                u.id AS UeId,
                                d.abreviacao as DreAbreviacao,
                                u.nome as UeNome,
                                u.tipo_escola as UeTipo
                            FROM
	                            ue u
                            INNER JOIN dre d ON
	                            d.id = u.dre_id
                            WHERE
	                            d.id = @dreId";
                if(ueId is not null)
                {
                    query += " AND u.id = @ueId";
                }

                query += ";";

                var resultado = await conn.QueryAsync<AbrangenciaUeDto>(query, new { dreId, ueId });
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AbrangenciaUeDto>> ObterUesAdministrador()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT
                                d.id AS DreId,
                                u.id AS UeId,
                                d.abreviacao as DreAbreviacao,
                                u.nome as UeNome,
                                u.tipo_escola as UeTipo
                            FROM
	                            ue u
                            INNER JOIN dre d ON
	                            d.id = u.dre_id;";

                var resultado = await conn.QueryAsync<AbrangenciaUeDto>(query);
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreAbragenciaDetalheDto>> ObterDresAbrangenciaPorLoginPerfil(string login, Guid perfil)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                    var query = @"select
	                                d.id,
	                                d.dre_id as codigo,
	                                d.abreviacao,
	                                d.nome
                                from abrangencia a
                                inner join usuario_serap_coresso usc ON usc.id = a.usuario_id
                                inner join grupo_serap_coresso gsc ON gsc.id = a.grupo_id
                                inner join dre d on d.id = a.dre_id
                                where usc.login = @login and gsc.id_coresso = @perfil
                                order by d.nome";

                var resultado = await conn.QueryAsync<DreAbragenciaDetalheDto>(query, new { login, perfil });
                return resultado;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreAbragenciaDetalheDto>> ObterDresAdministrador()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            d.id,
	                            d.dre_id,
	                            d.abreviacao,
	                            d.nome
                            from dre d
                            order by d.nome";

                var resultado = await conn.QueryAsync<DreAbragenciaDetalheDto>(query);
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

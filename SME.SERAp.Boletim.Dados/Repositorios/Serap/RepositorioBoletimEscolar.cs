using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioBoletimEscolar : RepositorioBase<BoletimEscolar>, IRepositorioBoletimEscolar
    {
        public RepositorioBoletimEscolar(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
        }

        public async Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long loteId, long ueId, FiltroBoletimDto filtros)

        {
            using var conn = ObterConexaoLeitura();

            try
            {

                var query = new StringBuilder(@"select
	                        be.id,
	                        be.ue_id,
	                        be.prova_id,
	                        be.componente_curricular,
	                        be.abaixo_basico,
	                        be.abaixo_basico_porcentagem,
	                        be.basico,
	                        be.basico_porcentagem,
	                        be.adequado,
	                        be.adequado_porcentagem,
	                        be.avancado,
	                        be.avancado_porcentagem,
	                        be.total,
	                        be.media_proficiencia
                        from
	                        boletim_escolar be
                        inner join boletim_lote_prova blp on
	                        blp.prova_id = be.prova_id 
                        inner join lote_prova lp on
	                        lp.id = blp.lote_id
                        where
	                        be.ue_id = @ueId and lp.id = @loteId 
                ");

                var parameters = new DynamicParameters();
                parameters.Add("loteId", loteId);
                parameters.Add("ueId", ueId);

                if (filtros?.ComponentesCurriculares?.Any() ?? false)
                {
                    var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                    query.Append(" AND  be.disciplina_id = ANY(@componentesCurriculares)");
                    parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtros?.Ano?.Any() ?? false)
                {
                    var anos = filtros.Ano.ToArray();
                    query.Append(@" AND CAST(regexp_replace(be.componente_curricular, '[^0-9]', '', 'g') AS INTEGER) = ANY(@anos)");
                    parameters.Add("anos", anos, DbType.Object);
                }

                query.Append(" order by be.componente_curricular asc");

                return await conn.QueryAsync<BoletimEscolar>(query.ToString(), parameters);

            }

            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarDto>> ObterDownloadProvasBoletimEscolar(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"SELECT 
	                                        bpa.ue_codigo CodigoUE,
                                            bpa.ue_nome NomeUE,
                                            bpa.ano_escolar AnoEscola,
                                            bpa.turma Turma,
                                            bpa.aluno_ra AlunoRA,
                                            bpa.aluno_nome NomeAluno,
                                            bpa.disciplina Componente,
                                            bpa.proficiencia Proficiencia,
                                            CONCAT(np.codigo, ' - ', np.descricao) Nivel
                                        FROM 
	                                        boletim_prova_aluno bpa
                                        INNER JOIN ue u ON
                             				u.ue_id = bpa.ue_codigo
                                        INNER JOIN nivel_proficiencia np on np.codigo  = bpa.nivel_codigo 
                                            and  np.disciplina_id = bpa.disciplina_id 
                                            and np.ano = bpa.ano_escolar
                                        INNER JOIN boletim_lote_prova blp ON 
	                                        blp.prova_id = bpa.prova_id 
                                        INNER JOIN lote_prova lp ON
	                                        lp.id = blp.lote_id
                                        WHERE
	                                        u.id = @ueId and lp.id = @loteId;";

                return await conn.QueryAsync<DownloadProvasBoletimEscolarDto>(query, new { loteId, ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaBoletimEscolarDto>> ObterProvasBoletimEscolarPorUe(long loteId, long ueId, FiltroBoletimDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder(@"SELECT 
	                            be.prova_id as Id,
	                            p.disciplina as Descricao
                            FROM 
	                            boletim_escolar be
                            INNER JOIN prova p ON
	                            p.id = be.prova_id
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = p.id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            WHERE
	                            be.ue_id = @ueId and lp.id = @loteId");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("loteId", loteId);

                if (filtros?.ComponentesCurriculares?.Any() ?? false)
                {
                    var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                    query.Append(" AND be.disciplina_id = ANY(@componentesCurriculares)");
                    parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtros?.Ano?.Any() ?? false)
                {
                    var anos = filtros.Ano.ToArray();
                    query.Append(@" AND CAST(regexp_replace(be.componente_curricular, '[^0-9]', '', 'g') AS INTEGER) = ANY(@anos)");
                    parameters.Add("anos", anos, DbType.Object);
                }

                query.Append(@" GROUP BY
	                                be.prova_id,
	                                p.disciplina
                                order by p.disciplina, be.prova_id;");

                return await conn.QueryAsync<ProvaBoletimEscolarDto>(query.ToString(), parameters);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadResultadoProbabilidade(long loteId, long ueId, long disciplinaId, int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"SELECT 
                                            brp.codigo_habilidade AS CodigoHabilidade,
                                            brp.habilidade_descricao AS HabilidadeDescricao,
                                            brp.turma_descricao AS TurmaDescricao,
                                            ROUND(brp.abaixo_do_basico, 2) AS AbaixoDoBasico,
                                            ROUND(brp.basico, 2) AS Basico,
                                            ROUND(brp.adequado, 2) AS Adequado,
                                            ROUND(brp.avancado, 2) AS Avancado
                                        FROM boletim_resultado_probabilidade brp
                                        INNER JOIN boletim_lote_prova blp ON
	                                        blp.prova_id = brp.prova_id
	                                    INNER JOIN lote_prova lp ON
	                                        lp.id = blp.lote_id
                                        WHERE brp.ue_id = @ueId
                                          AND lp.id = @loteId
                                          AND brp.disciplina_id = @disciplinaId
                                          AND brp.ano_escolar = @anoEscolar;";

                return await conn.QueryAsync<DownloadResultadoProbabilidadeDto>(query, new { loteId, ueId, disciplinaId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalUesPorDreAsync(long loteId, long dreId, int anoEscolar)
        {
            const string query = @"SELECT COUNT(DISTINCT bpa.ue_codigo) 
                                        FROM boletim_prova_aluno bpa
                                        INNER JOIN boletim_lote_prova blp ON blp.prova_id = bpa.prova_id
                                        WHERE bpa.dre_id = @dreId 
                                          AND bpa.ano_escolar = @anoEscolar 
                                          AND blp.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.ExecuteScalarAsync<int>(query, new { dreId, anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalAlunosPorDreAsync(long loteId, long dreId, int anoEscolar)
        {
            const string query = @"SELECT COUNT(DISTINCT bpa.aluno_ra) 
                                        FROM boletim_prova_aluno bpa
                                        INNER JOIN boletim_lote_prova blp ON blp.prova_id = bpa.prova_id
                                        WHERE bpa.dre_id = @dreId 
                                          AND bpa.ano_escolar = @anoEscolar 
                                          AND blp.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.ExecuteScalarAsync<int>(query, new { dreId, anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<MediaProficienciaDisciplinaDto>> ObterMediaProficienciaPorDreAsync(long loteId, long dreId, int anoEscolar)
        {
            const string query = @"SELECT 
                                          bpa.disciplina_id AS DisciplinaId,
                                          bpa.disciplina AS DisciplinaNome,
                                          ROUND(AVG(bpa.proficiencia), 2) AS MediaProficiencia
                                        FROM boletim_prova_aluno bpa
                                        INNER JOIN boletim_lote_prova blp ON blp.prova_id = bpa.prova_id
                                        WHERE 
                                          bpa.dre_id = @dreId 
                                          AND bpa.ano_escolar = @anoEscolar 
                                          AND blp.lote_id = @loteId
                                          AND bpa.proficiencia IS NOT NULL
                                        GROUP BY 
                                          bpa.disciplina_id, bpa.disciplina
                                        ORDER BY 
                                          bpa.disciplina_id;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.QueryAsync<MediaProficienciaDisciplinaDto>(query, new { dreId, anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UePorDreDto>> ObterUesPorDreAsync(long dreId, int anoEscolar, long loteId)
        {
            const string query = @"select
	                                u.id as UeId,
	                                u.nome as UeNome,
	                                u.tipo_escola as TipoEscola,
	                                d.id as DreId,
	                                d.abreviacao as drenomeabreviado,
	                                d.nome as DreNome
                                from
	                                boletim_lote_ue blu
                                inner join ue u on
	                                u.id = blu.ue_id
                                inner join dre d on
	                                d.id = blu.dre_id
                                where
	                                blu.dre_id = @dreId
	                                and blu.ano_escolar = @anoEscolar
	                                and blu.lote_id = @loteId
                                order by
	                                d.nome,
	                                u.nome";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.QueryAsync<UePorDreDto>(query, new { dreId, anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> ObterDownloadProvasBoletimEscolarPorDre(long dreId, long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"SELECT 
                                            d.dre_id as CodigoDre,
                                            d.abreviacao as NomeDreAbreviacao,
                                            bpa.ue_codigo CodigoUE,
                                            bpa.ue_nome NomeUE,
                                            bpa.ano_escolar AnoEscola,
                                            bpa.turma Turma,
                                            bpa.aluno_ra AlunoRA,
                                            bpa.aluno_nome NomeAluno,
                                            bpa.disciplina Componente,
                                            bpa.proficiencia Proficiencia,
                                            CONCAT(np.codigo, ' - ', np.descricao) Nivel
                                        FROM 
                                            boletim_prova_aluno bpa
                                        INNER JOIN dre d on d.id = bpa.dre_id  
                                        INNER JOIN ue u ON u.ue_id = bpa.ue_codigo
                                        INNER JOIN nivel_proficiencia np on np.codigo = bpa.nivel_codigo 
                                            and np.disciplina_id = bpa.disciplina_id 
                                            and np.ano = bpa.ano_escolar
                                        INNER JOIN boletim_lote_prova blp ON blp.prova_id = bpa.prova_id 
                                        INNER JOIN lote_prova lp ON lp.id = blp.lote_id
                                        WHERE bpa.dre_id = @dreId and lp.id = @loteId;";
                return await conn.QueryAsync<DownloadProvasBoletimEscolarPorDreDto>(query, new { dreId, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<PaginacaoUesBoletimDadosDto> ObterUesPorDre(long loteId, long dreId, int anoEscolar, FiltroUeBoletimDadosDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("loteId", loteId);
                parameters.Add("dreId", dreId);
                parameters.Add("anoEscolar", anoEscolar);

                var where = new StringBuilder(@"  where
	                                                blu.dre_id = @dreId
	                                                and blu.ano_escolar = @anoEscolar
	                                                and blu.lote_id = @loteId");

                if (filtros?.UesIds?.Any() ?? false)
                {
                    where.Append(" and u.id = ANY(@uesIds)");
                    parameters.Add("uesIds", filtros.UesIds.ToList(), DbType.Object);
                }

                var totalQuery = new StringBuilder(@"select
	                                                    count(u.id)
                                                    from
	                                                    boletim_lote_ue blu
                                                    inner join ue u on
	                                                    u.id = blu.ue_id
                                                inner join dre d on
	                                                d.id = blu.dre_id");

                totalQuery.Append(where);
                var totalRegistros = await conn.ExecuteScalarAsync<int>(totalQuery.ToString(), parameters);

                var query = new StringBuilder(@"select
                                                    u.id,
                                                    u.nome as ueNome,
                                                    u.tipo_escola as tipoEscola,
                                                    blu.ano_escolar as anoEscolar,
                                                    blu.total_alunos as totalEstudantes,
                                                    blu.realizaram_prova as totalEstudadesRealizaramProva,
                                                    d.id as DreId,
	                                                d.abreviacao as drenomeabreviado,
	                                                d.nome as DreNome
                                                from
                                                    boletim_lote_ue blu
                                                inner join ue u on
                                                    u.id = blu.ue_id
                                                inner join dre d on
	                                                d.id = blu.dre_id");
                
                query.Append(where);

                query.Append(@" order by
	                                u.nome
                                limit @limit offset @offset;");

                parameters.Add("offset", (filtros.Pagina - 1) * filtros.TamanhoPagina);
                parameters.Add("limit", filtros.TamanhoPagina);

                var ues = await conn.QueryAsync<UeDadosBoletimDto>(query.ToString(), parameters);

                return new PaginacaoUesBoletimDadosDto(ues, filtros.Pagina, filtros.TamanhoPagina, totalRegistros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UeBoletimDisciplinaProficienciaDto>> ObterDiciplinaMediaProficienciaProvaPorUes(long loteId, long dreId, int anoEscolar, IEnumerable<long> uesIds)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
	                                    distinct on
	                                    (be.ue_id,
	                                    be.disciplina_id,
	                                    p.disciplina,
	                                    pao.ano)
	                                    be.ue_id  as ueId,
	                                    p.disciplina,
	                                    be.disciplina_id as disciplinaid,
	                                    pao.ano as anoescolar,
	                                    be.media_proficiencia as mediaproficiencia,
	                                    be.nivel_ue_codigo as nivelCodigo,
	                                    be.nivel_ue_descricao as nivelDescricao,
	                                    blp.lote_id
                                    from
	                                    boletim_escolar be
                                    inner join ue u on u.id = be.ue_id
                                    inner join boletim_lote_prova blp on
	                                    blp.prova_id = be.prova_id
                                    inner join prova_ano_original pao on
	                                    pao.prova_id = be.prova_id
                                    inner join prova p on p.id = be.prova_id
                                    where
	                                    u.dre_id = @dreId
	                                    and pao.ano::int = @anoEscolar
	                                    and blp.lote_id = @loteId
                                        and u.id = ANY(@uesIds)     
	                                    and be.nivel_ue_codigo is not null
                                    order by
	                                    be.ue_id,
	                                    be.disciplina_id,
	                                    p.disciplina,
	                                    pao.ano,
	                                    be.id desc";

                var parameters = new DynamicParameters();
                parameters.Add("dreId", dreId);
                parameters.Add("anoEscolar", anoEscolar);
                parameters.Add("loteId", loteId);
                parameters.Add("uesIds", uesIds.ToList(), DbType.Object);

                return await conn.QueryAsync<UeBoletimDisciplinaProficienciaDto>(query, parameters);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalUes(long loteId, int anoEscolar)
        {
            const string query = @"select
	                                COUNT(distinct bpa.ue_codigo)
                                from
	                                boletim_prova_aluno bpa
                                inner join boletim_lote_prova blp on
	                                blp.prova_id = bpa.prova_id
                                where
	                                bpa.ano_escolar = @anoEscolar
	                                and blp.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.ExecuteScalarAsync<int>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalDres(long loteId, int anoEscolar)
        {
            const string query = @"select
	                                COUNT(distinct bpa.dre_id)
                                from
	                                boletim_prova_aluno bpa
                                inner join boletim_lote_prova blp on
	                                blp.prova_id = bpa.prova_id
                                where
	                                bpa.ano_escolar = @anoEscolar
	                                and blp.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.ExecuteScalarAsync<int>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalAlunos(long loteId, int anoEscolar)
        {
            const string query = @"select
	                                    COUNT(distinct bpa.aluno_ra)
                                    from
	                                    boletim_prova_aluno bpa
                                    inner join boletim_lote_prova blp on
	                                    blp.prova_id = bpa.prova_id
                                    where
	                                    bpa.ano_escolar = @anoEscolar
	                                    and blp.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.ExecuteScalarAsync<int>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<MediaProficienciaDisciplinaDto>> ObterMediaProficienciaGeral(long loteId, int anoEscolar)
        {
            const string query = @"select
	                                bpa.disciplina_id as DisciplinaId,
	                                bpa.disciplina as DisciplinaNome,
	                                ROUND(AVG(bpa.proficiencia), 2) as MediaProficiencia
                                from
	                                boletim_prova_aluno bpa
                                inner join boletim_lote_prova blp on
	                                blp.prova_id = bpa.prova_id
                                where
	                                bpa.ano_escolar = @anoEscolar
	                                and blp.lote_id = @loteId
	                                and bpa.proficiencia is not null
                                group by
	                                bpa.disciplina_id,
	                                bpa.disciplina
                                order by
	                                bpa.disciplina_id;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.QueryAsync<MediaProficienciaDisciplinaDto>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
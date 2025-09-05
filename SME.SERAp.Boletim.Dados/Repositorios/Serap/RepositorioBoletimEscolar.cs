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
            const string query = @"select
	                                    count(distinct blu.ue_id) 
                                    from
	                                    boletim_lote_ue blu
                                    where
	                                    blu.dre_id = @dreId
	                                    and blu.ano_escolar = @anoEscolar
	                                    and blu.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                var resultado = await conn.QueryAsync<int>(query, new { dreId, anoEscolar, loteId });
                return resultado?.FirstOrDefault() ?? 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalAlunosPorDreAsync(long loteId, long dreId, int anoEscolar)
        {
            const string query = @"select
	                                    sum(blu.realizaram_prova)
                                    from
	                                    boletim_lote_ue blu
                                    where
	                                    blu.dre_id = @dreId
	                                    and BLU.ano_escolar = @anoEscolar
	                                    and BLU.lote_id = @loteId
                                    group by
	                                    blu.dre_id;";

            using var conn = ObterConexaoLeitura();
            try
            {
                var resultado = await conn.QueryAsync<int>(query, new { dreId, anoEscolar, loteId });
                return resultado?.FirstOrDefault() ?? 0;
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
                                          bpa.disciplina;";

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
                var totalRegistros = await conn.QueryFirstOrDefaultAsync<int>(totalQuery.ToString(), parameters);

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
	                                    p.disciplina,
	                                    be.disciplina_id,
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
	                                    p.disciplina,
	                                    be.disciplina_id,
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
                var resultado = await conn.QueryAsync<int>(query, new { anoEscolar, loteId });
                return resultado?.FirstOrDefault() ?? 0;
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
                var resultado = await conn.QueryAsync<int>(query, new { anoEscolar, loteId });
                return resultado?.FirstOrDefault() ?? 0;
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
	                                    sum(blu.realizaram_prova) 
                                    from
	                                    boletim_lote_ue blu
                                    where
	                                     blu.ano_escolar = @anoEscolar
	                                    and blu.lote_id = @loteId;";

            using var conn = ObterConexaoLeitura();
            try
            {
                var resultado = await conn.QueryAsync<int>(query, new { anoEscolar, loteId });
                return resultado?.FirstOrDefault() ?? 0;
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

        public async Task<IEnumerable<DreDisciplinaMediaProficienciaDto>> ObterDresMediaProficienciaPorDisciplina(long loteId, long anoEscolar, IEnumerable<long> dresIds)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("loteId", loteId);
                parameters.Add("anoEscolar", anoEscolar.ToString());

                var query = new StringBuilder(@$"with uesDisciplinas as (
                                            select
	                                            d.id as dreid,
	                                            d.nome as drenome,
	                                            p.disciplina_id,
	                                            p.disciplina,
	                                            blp.prova_id,
	                                            blu.ano_escolar 
                                            from
	                                            boletim_lote_ue blu
                                            inner join dre d on
	                                            d.id = blu.dre_id
                                            inner join boletim_lote_prova blp on
	                                            blp.lote_id = blu.lote_id
                                            inner join prova p on 
	                                            p.id = blp.prova_id 
                                            inner join prova_ano_original pao on
	                                            pao.prova_id = p.id
                                            where
	                                            pao.ano  = @anoEscolar
	                                            and blu.lote_id = @loteId
                                            group by
	                                            d.id,
	                                            d.nome,
	                                            p.disciplina_id,
	                                            p.disciplina,
	                                            blp.prova_id,
	                                            blu.ano_escolar
                                            )
                                            select
	                                            ud.dreid,
	                                            ud.drenome,
	                                            ud.disciplina_id as disciplinaid,
	                                            ud.disciplina,
	                                            ud.prova_id as provaid,
	                                            coalesce(ROUND(AVG(bpa.proficiencia), 2), 0) as mediaproficiencia
                                            from
	                                            uesDisciplinas ud
                                            left join boletim_prova_aluno bpa 
                                                on
	                                            bpa.disciplina_id = ud.disciplina_id
	                                            and bpa.prova_id = ud.prova_id
	                                            and bpa.dre_id = ud.dreid
	                                            and bpa.ano_escolar = ud.ano_escolar");

                if (dresIds?.Any() ?? false)
                {
                    query.Append(" where ud.dreid = ANY(@dresIds)");
                    parameters.Add("dresIds", dresIds.ToArray(), DbType.Object);
                }

                query.Append(@" group by
	                                ud.dreid,
	                                ud.drenome,
	                                ud.disciplina_id,
	                                ud.disciplina,
	                                ud.prova_id
                                order by
	                                ud.drenome,
	                                ud.disciplina_id;");

                return await conn.QueryAsync<DreDisciplinaMediaProficienciaDto>(query.ToString(), parameters);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> ObterDownloadProvasBoletimEscolarSme(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
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
                                    from
	                                    boletim_prova_aluno bpa
                                    inner join dre d on
	                                    d.id = bpa.dre_id
                                    inner join ue u on
	                                    u.ue_id = bpa.ue_codigo
                                    inner join nivel_proficiencia np on
	                                    np.codigo = bpa.nivel_codigo
	                                    and np.disciplina_id = bpa.disciplina_id
	                                    and np.ano = bpa.ano_escolar
                                    inner join boletim_lote_prova blp on
	                                    blp.prova_id = bpa.prova_id
                                    inner join lote_prova lp on
	                                    lp.id = blp.lote_id
                                    where
	                                    lp.id = @loteId;";
                return await conn.QueryAsync<DownloadProvasBoletimEscolarPorDreDto>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadSmeResultadoProbabilidade(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
	                                    d.dre_id as CodigoDre,
	                                    d.abreviacao as NomeDreAbreviacao,
	                                    u.ue_id as CodigoUE,
	                                    u.nome as NomeUE,
	                                    brp.ano_escolar as AnoEscola,
	                                    brp.turma_descricao as TurmaDescricao,
	                                    p.disciplina as componente,
	                                    brp.codigo_habilidade as CodigoHabilidade,
	                                    brp.habilidade_descricao as HabilidadeDescricao,
	                                    ROUND(brp.abaixo_do_basico, 2) as AbaixoDoBasico,
	                                    ROUND(brp.basico, 2) as Basico,
	                                    ROUND(brp.adequado, 2) as Adequado,
	                                    ROUND(brp.avancado, 2) as Avancado
                                    from
	                                    boletim_resultado_probabilidade brp
                                    inner join boletim_lote_prova blp on
	                                    blp.prova_id = brp.prova_id
                                    inner join lote_prova lp on
	                                    lp.id = blp.lote_id
                                    inner join ue u on
	                                    u.id = brp.ue_id
                                    inner join dre d on
	                                    d.id = u.dre_id
                                    inner join prova p on
	                                    p.id = brp.prova_id 
                                    where
	                                    lp.id = @loteId
                                    order by
	                                    p.disciplina,
	                                    d.abreviacao,
	                                    u.nome,
	                                    brp.codigo_habilidade,
	                                    brp.turma_descricao";

                return await conn.QueryAsync<DownloadResultadoProbabilidadeDto>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreDto>> ObterDreAsync(int anoEscolar, long loteId)
        {
            const string query = @"select distinct
                                        d.id as DreId,
                                        d.nome as DreNome,
                                        d.abreviacao as DreNomeAbreviado
                                    from
                                        boletim_lote_ue blu
                                    inner join dre d on
                                        d.id = blu.dre_id
                                    where
                                        blu.ano_escolar = @anoEscolar
                                        and blu.lote_id = @loteId
                                    order by
                                        d.nome;";

            using var conn = ObterConexaoLeitura();
            try
            {
                return await conn.QueryAsync<DreDto>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadDreResultadoProbabilidade(long loteId, int dreId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
	                                    d.dre_id as CodigoDre,
	                                    d.abreviacao as NomeDreAbreviacao,
	                                    u.ue_id as CodigoUE,
	                                    u.nome as NomeUE,
	                                    brp.ano_escolar as AnoEscola,
	                                    brp.turma_descricao as TurmaDescricao,
	                                    p.disciplina as componente,
	                                    brp.codigo_habilidade as CodigoHabilidade,
	                                    brp.habilidade_descricao as HabilidadeDescricao,
	                                    ROUND(brp.abaixo_do_basico, 2) as AbaixoDoBasico,
	                                    ROUND(brp.basico, 2) as Basico,
	                                    ROUND(brp.adequado, 2) as Adequado,
	                                    ROUND(brp.avancado, 2) as Avancado
                                    from
	                                    boletim_resultado_probabilidade brp
                                    inner join boletim_lote_prova blp on
	                                    blp.prova_id = brp.prova_id
                                    inner join lote_prova lp on
	                                    lp.id = blp.lote_id
                                    inner join ue u on
	                                    u.id = brp.ue_id
                                    inner join dre d on
	                                    d.id = u.dre_id
                                    inner join prova p on
	                                    p.id = brp.prova_id 
                                    where
	                                    lp.id = @loteId
                                    and 
                                    	d.id = @dreId
                                    order by
	                                    p.disciplina,
	                                    d.abreviacao,
	                                    u.nome,
	                                    brp.codigo_habilidade,
	                                    brp.turma_descricao";

                return await conn.QueryAsync<DownloadResultadoProbabilidadeDto>(query, new { loteId, dreId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreResumoDto>> ObterResumoDreAsync(int anoEscolar, long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"
                                        select
	                                        d.id as DreId,
	                                        d.nome as DreNome,
	                                        blu.ano_escolar as AnoEscolar,
	                                        COUNT(distinct blu.ue_id) as TotalUes,
	                                        SUM(blu.total_alunos) as TotalAlunos,
	                                        SUM(blu.realizaram_prova) as TotalRealizaramProva
                                        from
	                                        boletim_lote_ue blu
                                        inner join dre d on
	                                        d.id = blu.dre_id
                                        where
	                                        blu.lote_id = @loteId
	                                        and blu.ano_escolar = @anoEscolar
                                        group by
	                                        d.id,
	                                        d.nome,
	                                        blu.ano_escolar;";

                return await conn.QueryAsync<DreResumoDto>(query, new { loteId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreMediaProficienciaDto>> ObterMediaProficienciaDreAsync(int anoEscolar, long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"
                                        select
	                                        bpa.dre_id as DreId,
	                                        bpa.disciplina as Disciplina,
	                                        bpa.disciplina_id as DisciplinaId,
	                                        coalesce(ROUND(AVG(bpa.proficiencia),
	                                        2),
	                                        0) as MediaProficiencia
                                        from
	                                        boletim_prova_aluno bpa
                                        inner join boletim_lote_prova blp on
	                                        blp.prova_id = bpa.prova_id
                                        where
	                                        bpa.ano_escolar = @anoEscolar
	                                        and blp.lote_id = @loteId
                                        group by
	                                        bpa.dre_id,
	                                        bpa.disciplina,
	                                        bpa.disciplina_id;";

                return await conn.QueryAsync<DreMediaProficienciaDto>(query, new { anoEscolar, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DreNivelProficienciaDto>> ObterNiveisProficienciaAsync(int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"
                                        select
	                                        disciplina_id as DisciplinaId,
	                                        ano as Ano,
	                                        descricao as Descricao,
	                                        valor_referencia as ValorReferencia
                                        from
	                                        nivel_proficiencia
                                        where
	                                        ano = @anoEscolar;";

                return await conn.QueryAsync<DreNivelProficienciaDto>(query, new { anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UeMediaProficienciaDto>> ObterMediaProficienciaUeAsync(long loteId, int ueId, int disciplinaId, int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
                                            blp.lote_id as LoteId,
                                            pao.ano, 
                                            be.disciplina_id,
                                            trim(regexp_replace(lp.nome, '\s*\([^)]*\)\s*$', '', 'g')) as NomeAplicacao,
                                            initcap(regexp_replace(lp.nome,
                                            '.*\(([^)]*)\).*',
                                            '\1')) as Periodo,  
                                            avg(be.media_proficiencia) as MediaProficiencia
                                        from
                                            boletim_escolar be
                                        inner join prova p on
                                            p.id = be.prova_id
                                        inner join boletim_lote_prova blp on
                                            blp.prova_id = p.id
                                        inner join lote_prova lp on
                                            lp.id = blp.lote_id
                                        inner join prova_ano_original pao on
                                         pao.prova_id = p.id 
                                        where
                                            be.ue_id = @ueId
                                            and be.disciplina_id = @disciplinaId
                                            and pao.ano = @anoEscolar
                                            and p.exibir_no_boletim = true
                                            and extract(year from p.fim) = (
                                                select
                                                    extract(year
                                                from
                                                    p2.fim)
                                                from
                                                    prova p2
                                                inner join boletim_lote_prova blp on
                                                    blp.prova_id = p2.id
                                                where
                                                    blp.lote_id = @loteId
                                                limit 1
                                            )
                                            group by
                                             blp.lote_id,
                                             pao.ano,
                                             be.disciplina_id,   
                                             lp.nome";

                return await conn.QueryAsync<UeMediaProficienciaDto>(query, new { loteId, ueId, disciplinaId, anoEscolar = anoEscolar.ToString() });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UeMediaProficienciaDto>> ObterMediaProficienciaUeAnoAnteriorAsync(long loteId, int ueId, int disciplinaId, int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"with ano_base as (
                                        select
	                                        extract(year
                                        from
	                                        p2.fim) as ano_corrente,
	                                        extract(year
                                        from
	                                        p2.fim) - 1 as ano_anterior
                                        from
	                                        prova p2
                                        inner join boletim_lote_prova blp2 on
	                                        blp2.prova_id = p2.id
                                        where
	                                        blp2.lote_id = @loteId
                                        limit 1
                                        )
                                        select
	                                        'Prova São Paulo' as NomeAplicacao,
	                                        0 as LoteId,
	                                        ab.ano_anterior as Periodo,
	                                        round(avg(app.proficiencia),
	                                        2) as MediaProficiencia
                                        from
	                                        boletim_prova_aluno bpa
                                        inner join ue u on
	                                        u.ue_id = bpa.ue_codigo
                                        inner join prova p on
	                                        p.id = bpa.prova_id
                                        inner join boletim_lote_prova blp on
	                                        blp.prova_id = p.id
                                        inner join lote_prova lp on
	                                        lp.id = blp.lote_id
                                        inner join ano_base ab on
	                                        true
                                        inner join aluno_prova_sp_proficiencia app 
                                            on
	                                        app.aluno_ra = bpa.aluno_ra
	                                        and app.disciplina_id = bpa.disciplina_id
	                                        and app.ano_letivo = ab.ano_anterior
                                        where
	                                        u.id = @ueId
	                                        and bpa.disciplina_id = @disciplinaId
	                                        and bpa.ano_escolar = @anoEscolar
	                                        and p.exibir_no_boletim = true
	                                        and bpa.proficiencia is not null
	                                        and extract(year
                                        from
	                                        p.fim) = ab.ano_corrente
                                        group by
	                                        bpa.ue_codigo,
	                                        ab.ano_anterior";

                return await conn.QueryAsync<UeMediaProficienciaDto>(query, new { loteId, ueId, disciplinaId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalAlunosRealizaramProvasUe(long loteId, int anoEscolar, int ueId)
        {
            const string query = @"select
	                                    blu.realizaram_prova as TotalAlunosRealizaramProva
                                    from
	                                    boletim_lote_ue blu
                                    inner join ue u on
	                                    u.id = blu.ue_id
                                    where
	                                    blu.lote_id = @loteId
	                                    and blu.ano_escolar = @anoEscolar
	                                    and u.id = @ueId";

            using var conn = ObterConexaoLeitura();
            try
            {
                var resultado = await conn.QueryAsync<int>(query, new { loteId, anoEscolar, ueId });
                return resultado?.FirstOrDefault() ?? 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ObterTotalAlunosUeRealizaramProvasSPAnterior(long loteId, int ueId, int disciplinaId, int anoEscolar)
        {
            const string query = @"with ano_base as (
                                    select
                                        extract(year
                                    from
                                        p2.fim) as ano_corrente,
                                        extract(year
                                    from
                                        p2.fim) - 1 as ano_anterior
                                    from
                                        prova p2
                                    inner join boletim_lote_prova blp2 on
                                        blp2.prova_id = p2.id
                                    where
                                        blp2.lote_id = @loteId
                                    limit 1
                                    )
                                    select
                                        count(distinct app.aluno_ra) 
                                    from
                                        boletim_prova_aluno bpa
                                    inner join ue u on
                                        u.ue_id = bpa.ue_codigo
                                    inner join prova p on
                                        p.id = bpa.prova_id
                                    inner join boletim_lote_prova blp on
                                        blp.prova_id = p.id
                                    inner join ano_base ab on
                                        true
                                    inner join aluno_prova_sp_proficiencia app 
                                        on
                                        app.aluno_ra = bpa.aluno_ra
                                        and app.disciplina_id = bpa.disciplina_id
                                        and app.ano_letivo = ab.ano_anterior
                                    where
                                        u.id = @ueId
                                        and bpa.disciplina_id = @disciplinaId
                                        and bpa.ano_escolar = @anoEscolar
                                        and p.exibir_no_boletim = true
                                        and bpa.proficiencia is not null
                                        and extract(year
                                    from
                                        p.fim) = ab.ano_corrente";

            using var conn = ObterConexaoLeitura();
            try
            {
                var resultado = await conn.QueryAsync<int>(query, new { loteId, ueId, disciplinaId, anoEscolar });
                return resultado?.FirstOrDefault() ?? 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ObterNivelProficienciaDto>> ObterNiveisProficienciaPorDisciplinaIdAsync(int disciplinaId, int anoEscolar)
        {
            using IDbConnection conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
                                            disciplina_id as DisciplinaId,
                                            ano as Ano,
                                            descricao as Descricao,
                                            valor_referencia as ValorReferencia
                                        from
                                            nivel_proficiencia
                                        where
                                            disciplina_id = @disciplinaId
                                        and
                                            ano = @anoEscolar";

                return await conn.QueryAsync<ObterNivelProficienciaDto>(query, new { disciplinaId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
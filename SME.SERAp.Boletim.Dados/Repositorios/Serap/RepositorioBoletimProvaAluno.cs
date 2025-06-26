using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;
using System.Text;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioBoletimProvaAluno : RepositorioBase<BoletimProvaAluno>, IRepositorioBoletimProvaAluno
    {
        public RepositorioBoletimProvaAluno(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long loteId, long ueId, long provaId, FiltroBoletimDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder(@"
                            SELECT 
                                ue.id,
                                bpa.prova_id,
                                bpa.turma,
                                count(case when bpa.nivel_codigo = 1 then 1 end) as abaixoBasico,
                                ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 1 THEN 1 END) * 100.0) / COUNT(*), 2) AS abaixoBasicoPorcentagem,
                                count(case when bpa.nivel_codigo = 2 then 1 end) as basico,
                                ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 2 THEN 1 END) * 100.0) / COUNT(*), 2) AS basicoPorcentagem,
                                count(case when bpa.nivel_codigo = 3 then 1 end) as adequado,
                                ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 3 THEN 1 END) * 100.0) / COUNT(*), 2) AS adequadoPorcentagem,
                                count(case when bpa.nivel_codigo = 4 then 1 end) as avancado,
                                ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 4 THEN 1 END) * 100.0) / COUNT(*), 2) AS avancadoPorcentagem,
                                count(*) as total,
                                ROUND(AVG(bpa.proficiencia), 2) AS mediaProficiencia
                            FROM
                                boletim_prova_aluno bpa
                            INNER JOIN ue ON
                                ue.ue_id = bpa.ue_codigo
                            INNER JOIN boletim_lote_prova blp ON 
                                blp.prova_id = bpa.prova_id 
                            INNER JOIN lote_prova lp ON
                                lp.id = blp.lote_id
                            WHERE
                                bpa.prova_id = @provaId and
                                ue.id = @ueId and lp.id = @loteId");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("loteId", loteId);
                parameters.Add("provaId", provaId);

                if (filtros?.ComponentesCurriculares?.Any() ?? false)
                {
                    var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                    query.Append(" AND bpa.disciplina_id = ANY(@componentesCurriculares)");
                    parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtros?.Ano?.Any() ?? false)
                {
                    var anos = filtros.Ano.ToArray();
                    query.Append(@" AND bpa.ano_escolar = ANY(@anos)");
                    parameters.Add("anos", anos, DbType.Object);
                }

                query.Append(@" GROUP BY
                                    ue.id,
                                    bpa.prova_id,
                                    bpa.turma;");

                return await conn.QueryAsync<TurmaBoletimEscolarDto>(query.ToString(), parameters);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long loteId, long ueId, long provaId, FiltroBoletimDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder(@"select
	                            bpa.ano_escolar as ano,
	                            np.codigo,
	                            np.descricao,
	                            np.valor_referencia as valor
                            from
                                boletim_prova_aluno bpa
                            inner join ue on
                                ue.ue_id = bpa.ue_codigo
                            inner join nivel_proficiencia np on
                                np.disciplina_id = bpa.disciplina_id and
                                np.ano = bpa.ano_escolar
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            where
                                bpa.prova_id = @provaId and
                                ue.id = @ueId and
                                lp.id = @loteId
                            ");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("provaId", provaId);
                parameters.Add("loteId", loteId);

                if (filtros?.ComponentesCurriculares?.Any() ?? false)
                {
                    var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                    query.Append(" AND bpa.disciplina_id = ANY(@componentesCurriculares)");
                    parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtros?.Ano?.Any() ?? false)
                {
                    var anos = filtros.Ano.ToArray();
                    query.Append(@" AND bpa.ano_escolar = ANY(@anos)");
                    parameters.Add("anos", anos, DbType.Object);
                }

                query.Append(@" GROUP BY
 	                                bpa.ano_escolar,
	                                np.codigo,
	                                np.descricao,
	                                np.valor_referencia
                                ORDER BY
	                                bpa.ano_escolar,
	                                np.codigo");

                return await conn.QueryAsync<NivelProficienciaBoletimEscolarDto>(query.ToString(), parameters);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
            ObterAbaEstudanteBoletimEscolarPorUeId(long loteId, long ueId, FiltroBoletimEstudantePaginadoDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var where = new StringBuilder(@" WHERE u.id = @ueId AND lp.id = @loteId");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("loteId", loteId);

                if (filtros?.ComponentesCurriculares?.Any() ?? false)
                {
                    var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                    where.Append(" AND bpa.disciplina_id = ANY(@componentesCurriculares)");
                    parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtros?.Ano?.Any() ?? false)
                {
                    var anos = filtros.Ano.ToArray();
                    where.Append(@" AND bpa.ano_escolar = ANY(@anos)");
                    parameters.Add("anos", anos, DbType.Object);
                }

                if (filtros?.Turma?.Any() ?? false)
                {
                    var turmas = filtros.Turma.ToArray();
                    where.Append(@" AND RIGHT(bpa.turma, 1) = ANY(@turmas)");
                    parameters.Add("turmas", turmas, DbType.Object);
                }

                if (filtros?.NivelProficiencia?.Any() ?? false)
                {
                    var niveisProficiencia = filtros.NivelProficiencia.ToArray();
                    where.Append(@" AND bpa.nivel_codigo = ANY(@niveisProficiencia)");
                    parameters.Add("niveisProficiencia", niveisProficiencia, DbType.Object);
                }

                if (filtros.NivelMinimo > 0)
                {
                    where.Append(@" AND bpa.proficiencia >= @nivelMinimo");
                    parameters.Add("nivelMinimo", filtros.NivelMinimo, DbType.Decimal);
                }

                if (filtros.NivelMaximo > 0)
                {
                    where.Append(@" AND bpa.proficiencia <= @nivelMaximo");
                    parameters.Add("nivelMaximo", filtros.NivelMaximo, DbType.Decimal);
                }

                if (!string.IsNullOrWhiteSpace(filtros.NomeEstudante))
                {
                    where.Append(@" AND bpa.aluno_nome ilike @nomeEstudante");
                    parameters.Add("nomeEstudante", $"%{filtros.NomeEstudante}%", DbType.String);
                }

                if (filtros.EolEstudante > 0)
                {
                    where.Append(@" AND bpa.aluno_ra = @eolEstudante");
                    parameters.Add("eolEstudante", filtros.EolEstudante, DbType.Int64);
                }

                var totalQuery = new StringBuilder(@"SELECT 
                                        COUNT(*) 
                                    FROM 
                                        boletim_prova_aluno bpa
                                    INNER JOIN ue u ON
	                                    u.ue_id = bpa.ue_codigo
                                    INNER JOIN boletim_lote_prova blp ON 
	                                    blp.prova_id = bpa.prova_id
                                    INNER JOIN lote_prova lp ON
	                                    lp.id = blp.lote_id");

                totalQuery.Append(where);
                var totalRegistros = await conn
                    .ExecuteScalarAsync<int>(totalQuery.ToString(), parameters);

                var query = new StringBuilder(@"SELECT 
                                    bpa.disciplina as disciplina,
                                    bpa.ano_escolar as anoescolar,
                                    bpa.turma as turma,
                                    bpa.aluno_ra as alunora, 
                                    bpa.aluno_nome as alunonome,
                                    bpa.proficiencia as proficiencia,
                                    bpa.nivel_codigo as nivelcodigo
                              FROM boletim_prova_aluno bpa
                              INNER JOIN ue u ON
	                            u.ue_id = bpa.ue_codigo
                              INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                              INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id");

                query.Append(where);
                query.Append(@" ORDER BY bpa.turma, bpa.aluno_nome
                              LIMIT @TamanhoPagina OFFSET @Offset");

                parameters.Add("TamanhoPagina", filtros.PageSize);
                parameters.Add("Offset", (filtros.PageNumber - 1) * filtros.PageSize);
                var estudantes = await conn
                    .QueryAsync<AbaEstudanteListaDto>(query.ToString(), parameters);

                return (estudantes, totalRegistros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            np.codigo as valor,
	                            np.descricao as texto
                            from
	                            boletim_prova_aluno bpa
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            inner join nivel_proficiencia np on
	                            np.codigo = bpa.nivel_codigo
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            where
	                            u.id = @ueId and lp.id = @loteId
                            group by
	                            np.codigo,
	                            np.descricao
                            order by  
	                            np.codigo";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { ueId, loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesAnoEscolarBoletimEscolarPorUeId(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            bpa.ano_escolar as valor,
	                            bpa.ano_escolar::text as texto
                            from
	                            boletim_prova_aluno bpa
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            where
	                            u.id = @ueId and lp.id = @loteId
                            group by
	                            bpa.ano_escolar
                            order by
	                            bpa.ano_escolar";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { loteId, ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            bpa.disciplina_id as valor,
	                            bpa.disciplina as texto
                            from
	                            boletim_prova_aluno bpa
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            where
	                            u.id = @ueId and lp.id = @loteId
                            group by
	                            bpa.disciplina_id,
	                            bpa.disciplina
                            order by
	                            bpa.disciplina_id,
	                            bpa.disciplina";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { loteId, ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<string>>> ObterOpcoesTurmaBoletimEscolarPorUeId(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            REGEXP_REPLACE(bpa.turma, '^\d', '') as valor,
	                            REGEXP_REPLACE(bpa.turma, '^\d', '') as texto
                            from
	                            boletim_prova_aluno bpa
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id
                            where
	                            u.id = @ueId and lp.id = @loteId
                            group by
	                            REGEXP_REPLACE(bpa.turma, '^\d', '')
                            order by
	                            valor";

                return await conn.QueryAsync<OpcaoFiltroDto<string>>(query, new { loteId, ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<BoletimEscolarValoresNivelProficienciaDto> ObterValoresNivelProficienciaBoletimEscolarPorUeId(long loteId, long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            min(bpa.proficiencia) as valorMinimo,
	                            max(bpa.proficiencia) as valorMaximo
                            from
	                            boletim_prova_aluno bpa
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            INNER JOIN boletim_lote_prova blp ON 
	                            blp.prova_id = bpa.prova_id
                            INNER JOIN lote_prova lp ON
	                            lp.id = blp.lote_id 
                            where
	                            u.id = @ueId and lp.id = @loteId";

                return await conn.QueryFirstAsync<BoletimEscolarValoresNivelProficienciaDto>(query, new { loteId, ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> ObterAbaEstudanteGraficoPorUeId(long loteId, long ueId, FiltroBoletimEstudanteDto filtros)
        {
            using var conn = ObterConexaoLeitura();

            var query = new StringBuilder(@"SELECT 
                            bpa.turma,
                            bpa.disciplina,
                            bpa.aluno_nome AS Nome,
                            bpa.proficiencia AS Proficiencia
                        FROM boletim_prova_aluno bpa
                        INNER JOIN ue u ON u.ue_id = bpa.ue_codigo
                        INNER JOIN boletim_lote_prova blp ON 
	                        blp.prova_id = bpa.prova_id
                        INNER JOIN lote_prova lp ON
	                        lp.id = blp.lote_id
                        WHERE u.id = @ueId
                        AND lp.id = @loteId");

            var parameters = new DynamicParameters();
            parameters.Add("ueId", ueId);
            parameters.Add("loteId", loteId);

            if (filtros?.ComponentesCurriculares?.Any() ?? false)
            {
                var componentesCorrigidos = filtros.ComponentesCurriculares.ToArray();
                query.Append(" AND bpa.disciplina_id = ANY(@componentesCurriculares)");
                parameters.Add("componentesCurriculares", componentesCorrigidos, DbType.Object);
            }

            if (filtros?.Ano?.Any() ?? false)
            {
                var anos = filtros.Ano.ToArray();
                query.Append(@" AND bpa.ano_escolar = ANY(@anos)");
                parameters.Add("anos", anos, DbType.Object);
            }

            if (filtros?.Turma?.Any() ?? false)
            {
                var turmas = filtros.Turma.ToArray();
                query.Append(@" AND RIGHT(bpa.turma, 1) = ANY(@turmas)");
                parameters.Add("turmas", turmas, DbType.Object);
            }

            if (filtros?.NivelProficiencia?.Any() ?? false)
            {
                var niveisProficiencia = filtros.NivelProficiencia.ToArray();
                query.Append(@" AND bpa.nivel_codigo = ANY(@niveisProficiencia)");
                parameters.Add("niveisProficiencia", niveisProficiencia, DbType.Object);
            }

            if (filtros.NivelMinimo > 0)
            {
                query.Append(@" AND bpa.proficiencia >= @nivelMinimo");
                parameters.Add("nivelMinimo", filtros.NivelMinimo, DbType.Decimal);
            }

            if (filtros.NivelMaximo > 0)
            {
                query.Append(@" AND bpa.proficiencia <= @nivelMaximo");
                parameters.Add("nivelMaximo", filtros.NivelMaximo, DbType.Decimal);
            }

            if (!string.IsNullOrWhiteSpace(filtros.NomeEstudante))
            {
                query.Append(@" AND bpa.aluno_nome ilike @nomeEstudante");
                parameters.Add("nomeEstudante", $"%{filtros.NomeEstudante}%", DbType.String);
            }

            query.Append(@" ORDER BY bpa.disciplina, bpa.turma, bpa.aluno_nome");

            var dados = await conn.QueryAsync<AbaEstudanteGraficoTempDto>(query.ToString(), parameters);

            return dados.GroupBy(d => new { d.Turma, d.Disciplina })
                        .Select(g => new AbaEstudanteGraficoDto
                        {
                            Turma = g.Key.Turma,
                            Disciplina = g.Key.Disciplina,
                            Alunos = g.Select(a => new AbaEstudanteGraficoAlunoDto
                            {
                                Nome = a.Nome,
                                Proficiencia = a.Proficiencia
                            }).ToList()
                        });
        }

        public async Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadePorUeAsync(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var where = new StringBuilder(@" WHERE brp.ue_id = @ueId 
                                    AND brp.disciplina_id = @disciplinaId 
                                    AND brp.ano_escolar = @anoEscolar");


                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("disciplinaId", disciplinaId);
                parameters.Add("anoEscolar", anoEscolar);

                if(filtros.Turma?.Any() ?? false)
                {
                    var turmas = filtros.Turma.ToArray();
                    where.Append(@" AND RIGHT(brp.turma_descricao, 1) = ANY(@turmas)");
                    parameters.Add("turmas", turmas, DbType.Object);
                }

                if (!string.IsNullOrWhiteSpace(filtros.Habilidade))
                {
                    where.Append(@" AND (brp.codigo_habilidade ilike @habilidade or brp.habilidade_descricao ilike @habilidade)");
                    parameters.Add("habilidade", $"%{filtros.Habilidade}%", DbType.String);
                }

                var totalQuery = new StringBuilder(@"SELECT 
                                                        COUNT(DISTINCT brp.codigo_habilidade)
                                                    FROM 
                                                        boletim_resultado_probabilidade brp
                                                    INNER JOIN boletim_lote_prova blp ON 
	                                                    blp.prova_id = brp.prova_id
                                                    INNER JOIN lote_prova lp ON
	                                                    lp.id = blp.lote_id AND
	                                                    lp.exibir_no_boletim");

                totalQuery.Append(where);
                var totalRegistros = await conn.ExecuteScalarAsync<int>(totalQuery.ToString(), parameters);


                var query = new StringBuilder(@"
                                WITH HabilidadesFiltradas AS (
                                    SELECT DISTINCT brp.codigo_habilidade, brp.habilidade_descricao
                                    FROM boletim_resultado_probabilidade brp
                                    INNER JOIN boletim_lote_prova blp ON 
	                                    blp.prova_id = brp.prova_id
                                    INNER JOIN lote_prova lp ON
	                                    lp.id = blp.lote_id AND
	                                    lp.exibir_no_boletim");

                query.Append(where);

                query.Append(@" ORDER BY brp.codigo_habilidade
                               LIMIT @limit OFFSET @offset)");

                parameters.Add("offset", (filtros.Pagina - 1) * filtros.TamanhoPagina);
                parameters.Add("limit", filtros.TamanhoPagina);

                var selectQuery = new StringBuilder(@" SELECT 
                                    brp.codigo_habilidade AS CodigoHabilidade,
                                    brp.habilidade_descricao AS HabilidadeDescricao,
                                    brp.turma_descricao AS TurmaDescricao,
                                    ROUND(brp.abaixo_do_basico, 2)  AS AbaixoDoBasico,
                                    ROUND(brp.basico, 2) AS Basico,
                                    ROUND(brp.adequado, 2) AS Adequado,
                                    ROUND(brp.avancado, 2) AS Avancado
                                FROM boletim_resultado_probabilidade brp
                                INNER JOIN HabilidadesFiltradas hf ON brp.codigo_habilidade = hf.codigo_habilidade
                                INNER JOIN boletim_lote_prova blp ON 
	                                blp.prova_id = brp.prova_id
                                INNER JOIN lote_prova lp ON
	                                lp.id = blp.lote_id AND
	                                lp.exibir_no_boletim");
                selectQuery.Append(where);
                selectQuery.Append(@" ORDER BY brp.codigo_habilidade, brp.turma_descricao;");
                query.Append(selectQuery);

                var resultados = await conn.QueryAsync<ResultadoProbabilidadeDto>(query.ToString(), parameters);

                return (resultados, totalRegistros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadeListaPorUeAsync(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var where = new StringBuilder(@" WHERE brp.ue_id = @ueId 
                                    AND brp.disciplina_id = @disciplinaId 
                                    AND brp.ano_escolar = @anoEscolar");


                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
                parameters.Add("disciplinaId", disciplinaId);
                parameters.Add("anoEscolar", anoEscolar);

                if (filtros.Turma?.Any() ?? false)
                {
                    var turmas = filtros.Turma.ToArray();
                    where.Append(@" AND RIGHT(brp.turma_descricao, 1) = ANY(@turmas)");
                    parameters.Add("turmas", turmas, DbType.Object);
                }

                if (!string.IsNullOrWhiteSpace(filtros.Habilidade))
                {
                    where.Append(@" AND (brp.codigo_habilidade ilike @habilidade or brp.habilidade_descricao ilike @habilidade)");
                    parameters.Add("habilidade", $"%{filtros.Habilidade}%", DbType.String);
                }

                var totalQuery = new StringBuilder(@"SELECT 
                                                        COUNT(*)
                                                    FROM 
                                                        boletim_resultado_probabilidade brp
                                                    INNER JOIN boletim_lote_prova blp ON 
	                                                    blp.prova_id = brp.prova_id
                                                    INNER JOIN lote_prova lp ON
	                                                    lp.id = blp.lote_id AND
	                                                    lp.exibir_no_boletim");

                totalQuery.Append(where);
                var totalRegistros = await conn.ExecuteScalarAsync<int>(totalQuery.ToString(), parameters);

                var query = new StringBuilder(@"SELECT 
                                brp.codigo_habilidade AS CodigoHabilidade,
                                brp.habilidade_descricao AS HabilidadeDescricao,
                                brp.turma_descricao AS TurmaDescricao,
                                ROUND(brp.abaixo_do_basico, 2)  AS AbaixoDoBasico,
                                ROUND(brp.basico, 2) AS Basico,
                                ROUND(brp.adequado, 2) AS Adequado,
                                ROUND(brp.avancado, 2) AS Avancado
                            FROM boletim_resultado_probabilidade brp
                            INNER JOIN boletim_lote_prova blp ON 
                                blp.prova_id = brp.prova_id
                            INNER JOIN lote_prova lp ON
                                lp.id = blp.lote_id AND
                                lp.exibir_no_boletim");

                query.Append(where);
                query.Append(@" ORDER BY brp.codigo_habilidade, brp.turma_descricao
                                      LIMIT @limit OFFSET @offset;");

                parameters.Add("offset", (filtros.Pagina - 1) * filtros.TamanhoPagina);
                parameters.Add("limit", filtros.TamanhoPagina);

                var resultados = await conn.QueryAsync<ResultadoProbabilidadeDto>(query.ToString(), parameters);

                return (resultados, totalRegistros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
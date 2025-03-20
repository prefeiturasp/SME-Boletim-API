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

        public async Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long ueId, long provaId, FiltroBoletimDto filtros)
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
                            WHERE
                                bpa.prova_id = @provaId and
                                ue.id = @ueId");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
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

        public async Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long ueId, long provaId, FiltroBoletimDto filtros)
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
                            where
                                bpa.prova_id = @provaId and
                                ue.id = @ueId
                            ");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);
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
            ObterAbaEstudanteBoletimEscolarPorUeId(long ueId, FiltroBoletimEstudantePaginadoDto filtros)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var where = new StringBuilder(@" WHERE u.id = @ueId");

                var parameters = new DynamicParameters();
                parameters.Add("ueId", ueId);

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
	                                    u.ue_id = bpa.ue_codigo");

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
	                            u.ue_id = bpa.ue_codigo");

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

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(long ueId)
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
                            where
	                            u.id = @ueId
                            group by
	                            np.codigo,
	                            np.descricao
                            order by  
	                            np.codigo";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesAnoEscolarBoletimEscolarPorUeId(long ueId)
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
                            where
	                            u.id = @ueId
                            group by
	                            bpa.ano_escolar
                            order by
	                            bpa.ano_escolar";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(long ueId)
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
                            where
	                            u.id = @ueId
                            group by
	                            bpa.disciplina_id,
	                            bpa.disciplina
                            order by
	                            bpa.disciplina_id,
	                            bpa.disciplina";

                return await conn.QueryAsync<OpcaoFiltroDto<int>>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<OpcaoFiltroDto<string>>> ObterOpcoesTurmaBoletimEscolarPorUeId(long ueId)
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
                            where
	                            u.id = @ueId
                            group by
	                            REGEXP_REPLACE(bpa.turma, '^\d', '')
                            order by
	                            valor";

                return await conn.QueryAsync<OpcaoFiltroDto<string>>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<BoletimEscolarValoresNivelProficienciaDto> ObterValoresNivelProficienciaBoletimEscolarPorUeId(long ueId)
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
                            where
	                            u.id = @ueId";

                return await conn.QueryFirstAsync<BoletimEscolarValoresNivelProficienciaDto>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> ObterAbaEstudanteGraficoPorUeId(long ueId, FiltroBoletimEstudanteDto filtros)
        {
            using var conn = ObterConexaoLeitura();

            var query = new StringBuilder(@"SELECT 
                            bpa.turma,
                            bpa.disciplina,
                            bpa.aluno_nome AS Nome,
                            bpa.proficiencia AS Proficiencia
                        FROM boletim_prova_aluno bpa
                        INNER JOIN ue u ON u.ue_id = bpa.ue_codigo
                        WHERE u.id = @ueId");

            var parameters = new DynamicParameters();
            parameters.Add("ueId", ueId);

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

            query.Append(@" ORDER BY bpa.turma, bpa.disciplina, bpa.aluno_nome");

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
    }
}
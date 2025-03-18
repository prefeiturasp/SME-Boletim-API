using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioBoletimProvaAluno : RepositorioBase<BoletimProvaAluno>, IRepositorioBoletimProvaAluno
    {
        public RepositorioBoletimProvaAluno(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long ueId, long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
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
                            from
                                boletim_prova_aluno bpa
                            inner join ue on
                                ue.ue_id = bpa.ue_codigo
                            where
                                bpa.prova_id = @provaId and
                                ue.id = @ueId
                            group by
                                ue.id,
                                bpa.prova_id,
                                bpa.turma;";

                return await conn.QueryAsync<TurmaBoletimEscolarDto>(query, new { ueId, provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long ueId, long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
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
                            group by
 	                            bpa.ano_escolar,
	                            np.codigo,
	                            np.descricao,
	                            np.valor_referencia
                            order by
	                            bpa.ano_escolar,
	                            np.codigo";

                return await conn.QueryAsync<NivelProficienciaBoletimEscolarDto>(query, new { ueId, provaId });
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
    }
}

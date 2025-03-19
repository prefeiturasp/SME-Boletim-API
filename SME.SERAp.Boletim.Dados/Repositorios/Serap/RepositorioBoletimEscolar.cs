﻿using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;
using System.Text;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioBoletimEscolar : RepositorioBase<BoletimEscolar>, IRepositorioBoletimEscolar
    {
        public RepositorioBoletimEscolar(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
        }

        public async Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long ueId, FiltroBoletimDto filtro)

        {
            using var conn = ObterConexaoLeitura();

            try
            {

                var query = new StringBuilder(@"
                SELECT id, ue_id, prova_id, componente_curricular, 
                       abaixo_basico, abaixo_basico_porcentagem, 
                       basico, basico_porcentagem, 
                       adequado, adequado_porcentagem, 
                       avancado, avancado_porcentagem, 
                       total, media_proficiencia  
                 FROM boletim_escolar 
                 WHERE ue_id = @ueId
            ");

                var parameters = new DynamicParameters();

                // Dicionário de mapeamento dos nomes das disciplinas
                var mapeamentoDisciplinas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                   { "matematica", "Matemática" },
                   { "linguaPortuguesa", "Língua Portuguesa" }
                 };


                parameters.Add("ueId", ueId);


                // Aplicar filtros dinamicamente


                if (filtro.ComponentesCurriculares.Any())
                {
                    var componentesCorrigidos = filtro.ComponentesCurriculares
                   .Select(c => mapeamentoDisciplinas.ContainsKey(c) ? mapeamentoDisciplinas[c] : c).Select(c => $"%{c}%").ToArray();


                    query.Append(" AND SPLIT_PART(componente_curricular, '(', 1) ILIKE ANY(@ComponentesCurriculares)");
                    parameters.Add("ComponentesCurriculares", componentesCorrigidos, DbType.Object);
                }

                if (filtro.Ano.Any())
                {
                    var anos = filtro.Ano.ToArray(); // Converte para array

                    query.Append(@"AND CAST(regexp_replace(componente_curricular, '[^0-9]', '', 'g') AS INTEGER) = ANY(@anos)");

                    parameters.Add("Anos", anos, DbType.Object);
                }


                return await conn.QueryAsync<BoletimEscolar>(query.ToString(), parameters);

            }

            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarDto>> ObterDownloadProvasBoletimEscolar(long ueId)
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
                                        WHERE
	                                        u.id = @ueId;";

                return await conn.QueryAsync<DownloadProvasBoletimEscolarDto>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaBoletimEscolarDto>> ObterProvasBoletimEscolarPorUe(long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select 
	                            be.prova_id as Id,
	                            p.disciplina as Descricao
                            from 
	                            boletim_escolar be
                            inner join prova p on
	                            p.id = be.prova_id
                            where
	                            be.ue_id = @ueId
                            group by
	                            be.prova_id,
	                            p.disciplina;";

                return await conn.QueryAsync<ProvaBoletimEscolarDto>(query, new { ueId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}

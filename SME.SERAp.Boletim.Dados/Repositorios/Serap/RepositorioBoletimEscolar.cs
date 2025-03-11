using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Repositorios.Serap
{
    public class RepositorioBoletimEscolar : RepositorioBase<BoletimEscolar>, IRepositorioBoletimEscolar
    {
        public RepositorioBoletimEscolar(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
        }

        public async Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"SELECT * FROM boletim_escolar WHERE ue_id = @ueId;";
                return await conn.QueryAsync<BoletimEscolar>(query, new { ueId });
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
	                            be.ue_id = @ueId";

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
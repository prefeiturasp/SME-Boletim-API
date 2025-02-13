using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Aluno;
using SME.SERAp.Boletim.Infra.Dtos.Autenticacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioAluno : IRepositorioBase<Aluno>
    {
        Task<ObterAlunoAtivoRetornoDto> ObterAlunoAtivoPorRa(long ra);
        Task<AlunoDetalheDto> ObterAlunoDetalhePorRa(long ra);
        Task<Aluno> ObterPorRA(long ra);
    }
}
using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Aluno;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAlunoPorRaUseCase : IObterAlunoPorRaUseCase
    {
        private readonly IMediator mediator;

        public ObterAlunoPorRaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Aluno> Executar(long alunoRA)
        {
            var alunoDetalhes = await mediator.Send(new ObterAlunoSerapPorRaQuery(alunoRA));

            if (alunoDetalhes != null)
                return alunoDetalhes;

            throw new NegocioException($"Não foi possível localizar os dados do aluno {alunoRA}");
        }
    }
}
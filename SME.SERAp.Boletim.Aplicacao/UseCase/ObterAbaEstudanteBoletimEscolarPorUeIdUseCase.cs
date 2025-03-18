using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAbaEstudanteBoletimEscolarPorUeIdUseCase : IObterAbaEstudanteBoletimEscolarPorUeIdUseCase
    {
        private readonly IMediator mediator;

        public ObterAbaEstudanteBoletimEscolarPorUeIdUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarComDisciplinasDto> Executar(string ueId, int pagina, int tamanhoPagina)
        {
            var (estudanteDetalhes, totalRegistros) = await mediator.Send(new ObterAbaEstudanteBoletimEscolarPorUeIdQuery(ueId, pagina, tamanhoPagina));

            if (estudanteDetalhes == null || !estudanteDetalhes.Any())
            {
                var erro = new RetornoBaseDto($"Não foi possível localizar os dados dos estudantes para o UE {ueId}");
                throw new NegocioException(erro.Mensagens.First());
            }

            var disciplinas = estudanteDetalhes
                .Select(e => e.Disciplina)
                .Distinct()
                .ToList();

            return new BoletimEscolarComDisciplinasDto
            {
                Disciplinas = disciplinas,
                Estudantes = new PaginacaoDto<AbaEstudanteListaDto>(estudanteDetalhes, pagina, tamanhoPagina, totalRegistros)
            };
        }
    }
}
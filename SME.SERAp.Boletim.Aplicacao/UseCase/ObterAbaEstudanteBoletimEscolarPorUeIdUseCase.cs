using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
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

        public async Task<BoletimEscolarComDisciplinasDto> Executar(long ueId, int pagina, int tamanhoPagina)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var (estudanteDetalhes, totalRegistros) = await mediator.Send(new ObterAbaEstudanteBoletimEscolarPorUeIdQuery(ueId, pagina, tamanhoPagina));

            if (estudanteDetalhes == null || !estudanteDetalhes.Any())
            {
                return new BoletimEscolarComDisciplinasDto
                {
                    Disciplinas = new List<string>(), // Retorna uma lista vazia
                    Estudantes = new PaginacaoDto<AbaEstudanteListaDto>(new List<AbaEstudanteListaDto>(), pagina, tamanhoPagina, 0)
                };
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
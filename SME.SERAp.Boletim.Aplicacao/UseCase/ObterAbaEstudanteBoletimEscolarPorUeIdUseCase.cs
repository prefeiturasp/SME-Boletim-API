using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAbaEstudanteBoletimEscolarPorUeIdUseCase : IObterAbaEstudanteBoletimEscolarPorUeIdUseCase
    {
        private readonly IMediator mediator;

        public ObterAbaEstudanteBoletimEscolarPorUeIdUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarComDisciplinasDto> Executar(long loteId, long ueId, FiltroBoletimEstudantePaginadoDto filtros)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var (estudanteDetalhes, totalRegistros) = await mediator.Send(new ObterAbaEstudanteBoletimEscolarPorUeIdQuery(loteId, ueId, filtros));

            if (estudanteDetalhes == null || !estudanteDetalhes.Any())
            {
                return new BoletimEscolarComDisciplinasDto
                {
                    Disciplinas = new List<string>(), // Retorna uma lista vazia
                    Estudantes = new PaginacaoDto<AbaEstudanteListaDto>(new List<AbaEstudanteListaDto>(), filtros.PageNumber, filtros.PageSize, 0)
                };
            }

            var disciplinas = estudanteDetalhes
                .Select(e => e.Disciplina)
                .Distinct()
                .ToList();

            return new BoletimEscolarComDisciplinasDto
            {
                Disciplinas = disciplinas,
                Estudantes = new PaginacaoDto<AbaEstudanteListaDto>(estudanteDetalhes, filtros.PageNumber, filtros.PageSize, totalRegistros)
            };
        }
    }
}
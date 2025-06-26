using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarOpcoesFiltrosPorUeUseCase
        : AbstractUseCase, IObterBoletimEscolarOpcoesFiltrosPorUeUseCase
    {
        public ObterBoletimEscolarOpcoesFiltrosPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<BoletimEscolarOpcoesFiltrosDto> Executar(long loteId, long ueId)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var niveis = await ObterNiveis(loteId, ueId);
            var anosEscolares = await ObterAnosEscolares(loteId, ueId);
            var componentesCurriculares = await ObterComponentesCurriculares(loteId, ueId);
            var turmas = await ObterTurmas(loteId, ueId);
            var boletimValoresProficiencia = await ObterBoletimValoresProficiencia(loteId, ueId);

            var opcoesFiltros = new BoletimEscolarOpcoesFiltrosDto
            {
                Niveis = niveis,
                AnosEscolares = anosEscolares,
                ComponentesCurriculares = componentesCurriculares,
                Turmas = turmas,
                NivelMinimo = boletimValoresProficiencia.ValorMinimo,
                NivelMaximo = boletimValoresProficiencia.ValorMaximo
            };

            return opcoesFiltros;
        }

        private async Task<BoletimEscolarValoresNivelProficienciaDto> ObterBoletimValoresProficiencia(long loteId, long ueId)
        {
            var boletimValoresProficiencia = await mediator
                            .Send(new ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId));

            if (boletimValoresProficiencia is not null)
            {
                boletimValoresProficiencia.ValorMinimo = TratarValorProficienciaMinima(boletimValoresProficiencia.ValorMinimo);
                boletimValoresProficiencia.ValorMaximo = TratarValorProficienciaMaxima(boletimValoresProficiencia.ValorMaximo);
            }

            return boletimValoresProficiencia ?? new();
        }

        private decimal TratarValorProficienciaMinima(decimal valorMinimo)
        {
            return (int)(Math.Floor(valorMinimo / 25) * 25);
        }

        private decimal TratarValorProficienciaMaxima(decimal valorMaxima)
        {
            return (int)(Math.Ceiling(valorMaxima / 25) * 25);
        }

        private async Task<IEnumerable<OpcaoFiltroDto<string>>> ObterTurmas(long loteId, long ueId)
        {
            return await mediator
                            .Send(new ObterOpcoesTurmaBoletimEscolarPorUeIdQuery(loteId, ueId))
                            ?? new List<OpcaoFiltroDto<string>>();
        }

        private async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterComponentesCurriculares(long loteId, long ueId)
        {
            return await mediator
                            .Send(new ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery(loteId, ueId))
                            ?? new List<OpcaoFiltroDto<int>>();
        }

        private async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterAnosEscolares(long loteId, long ueId)
        {
            return await mediator
                            .Send(new ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery(loteId, ueId))
                            ?? new List<OpcaoFiltroDto<int>>();
        }

        private async Task<IEnumerable<OpcaoFiltroDto<int>>> ObterNiveis(long loteId, long ueId)
        {
            return await mediator
                            .Send(new ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery(loteId, ueId))
                            ?? new List<OpcaoFiltroDto<int>>();
        }
    }
}

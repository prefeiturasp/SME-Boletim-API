using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarTurmaPorUeUseCase : IObterBoletimEscolarTurmaPorUeUseCase
    {
        private readonly IMediator mediator;
        public ObterBoletimEscolarTurmaPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarPorTurmaDto> Executar(long ueId)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var provasBoletimEscola = await mediator.Send(new ObterProvasBoletimEscolarPorUeQuery(ueId));
            if (provasBoletimEscola?.Any() ?? false)
            {
                foreach (var prova in provasBoletimEscola)
                {
                    var turmas = await ObterBoletinsEscolaresTurmas(ueId, prova);
                    prova.Turmas = turmas;

                    var niveis = await ObterNiveisProficiencia(ueId, prova);
                    prova.Niveis = niveis;
                }
            }

            return new BoletimEscolarPorTurmaDto(provasBoletimEscola);
        }

        private async Task<IEnumerable<ProvaTurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmas(long ueId, ProvaBoletimEscolarDto prova)
        {
            var provaTurmasBoletim = new List<ProvaTurmaBoletimEscolarDto>();
            var turmasBoletim =  await mediator.Send(new ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(ueId, prova.Id));

            if (turmasBoletim?.Any() ?? false)
            {
                foreach (var turmaBoletim in turmasBoletim)
                {
                    provaTurmasBoletim.Add(new ProvaTurmaBoletimEscolarDto
                    {
                        Turma = turmaBoletim.Turma,
                        Total = turmaBoletim.Total,
                        AbaixoBasico = ObterValorComPorcentagemNivelProficiencia(turmaBoletim.AbaixoBasico, turmaBoletim.AbaixoBasicoPorcentagem),
                        Basico = ObterValorComPorcentagemNivelProficiencia(turmaBoletim.Basico, turmaBoletim.BasicoPorcentagem),
                        Adequado = ObterValorComPorcentagemNivelProficiencia(turmaBoletim.Adequado, turmaBoletim.AdequadoPorcentagem),
                        Avancado = ObterValorComPorcentagemNivelProficiencia(turmaBoletim.Avancado, turmaBoletim.AvancadoPorcentagem),
                        MediaProficiencia = turmaBoletim.MediaProficiencia
                    });
                }
            }

            return provaTurmasBoletim;
        }

        private static string ObterValorComPorcentagemNivelProficiencia(decimal valor, decimal porcentagem)
        {
            return $"{valor} ({porcentagem:0.##}%)";
        }

        public async Task<IEnumerable<ProvaNivelProficienciaBoletimEscolarDto>> ObterNiveisProficiencia(long ueId, ProvaBoletimEscolarDto prova)
        {
            var provaNiveisProficiencia = new List<ProvaNivelProficienciaBoletimEscolarDto>();
            var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(ueId, prova.Id));

            if (niveisProficiencia?.Any() ?? false)
            {
                var grupoNiveisPorAnoEscolar = niveisProficiencia.GroupBy(x => x.Ano).OrderBy(x => x.Key);
                foreach (var grupoNivelAnoEscolar in grupoNiveisPorAnoEscolar)
                {
                    provaNiveisProficiencia.Add(new ProvaNivelProficienciaBoletimEscolarDto
                    {
                        AnoEscolar = grupoNivelAnoEscolar.Key,
                        AbaixoBasico = ObterDescricaoNivelProficiencia(TipoNivelProficiencia.AbaixoBasico, grupoNivelAnoEscolar),
                        Basico = ObterDescricaoNivelProficiencia(TipoNivelProficiencia.Basico, grupoNivelAnoEscolar),
                        Adequado = ObterDescricaoNivelProficiencia(TipoNivelProficiencia.Adequado, grupoNivelAnoEscolar),
                        Avancado = ObterDescricaoNivelProficiencia(TipoNivelProficiencia.Avancado, grupoNivelAnoEscolar),
                    });
                }
            }

            return provaNiveisProficiencia;
        }

        private static long? ObterValorNivelProficiencia(TipoNivelProficiencia tipoNivelProficiencia, IGrouping<int, NivelProficienciaBoletimEscolarDto> grupoNivelAnoEscolar)
        {
            return grupoNivelAnoEscolar.FirstOrDefault(x => x.Codigo == (int)tipoNivelProficiencia)?.Valor;
        }

        private static Dictionary<TipoNivelProficiencia, Func<IGrouping<int, NivelProficienciaBoletimEscolarDto>, string>> DescricoesNivelProficiencia() => new()
        {
            { TipoNivelProficiencia.AbaixoBasico, (grupo) => $"<{ObterValorNivelProficiencia(TipoNivelProficiencia.AbaixoBasico, grupo)}" },
            { TipoNivelProficiencia.Basico, (grupo) => $">={ObterValorNivelProficiencia(TipoNivelProficiencia.AbaixoBasico, grupo)} e <{ObterValorNivelProficiencia(TipoNivelProficiencia.Basico, grupo)}" },
            { TipoNivelProficiencia.Adequado, (grupo) => $">={ObterValorNivelProficiencia(TipoNivelProficiencia.Basico, grupo)} e <{ObterValorNivelProficiencia(TipoNivelProficiencia.Adequado, grupo)}" },
            { TipoNivelProficiencia.Avancado, (grupo) => $">={ObterValorNivelProficiencia(TipoNivelProficiencia.Adequado, grupo)}" }
        };

        private static string ObterDescricaoNivelProficiencia(TipoNivelProficiencia tipoNivelProficiencia, IGrouping<int, NivelProficienciaBoletimEscolarDto> grupoNivelAnoEscolar)
        {
            if (DescricoesNivelProficiencia().TryGetValue(tipoNivelProficiencia, out var strategy))
            {
                return strategy(grupoNivelAnoEscolar);
            }

            return string.Empty;
        }
    }
}

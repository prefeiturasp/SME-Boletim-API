using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaUes;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterUesPorNivelProficienciaDisciplinaPorDreUseCase : IObterUesPorNivelProficienciaDisciplinaPorDreUseCase
    {
        private readonly IMediator mediator;
        public ObterUesPorNivelProficienciaDisciplinaPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<DreResumoUesNivelProficienciaDto> Executar(long loteId, long dreId, int anoEscolar)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            var resultado = new DreResumoUesNivelProficienciaDto
            {
                AnoEscolar = anoEscolar,
                LoteId = loteId,
                DreId = dreId,
                Disciplinas = new List<DisciplinaResumoUesNivelProficienciaDto>()
            };

            var uesPorNiveis = await mediator.Send(new ObterNiveisProficienciaUesQuery(dreId, anoEscolar, loteId));
            if (uesPorNiveis == null || !uesPorNiveis.Any())
                return resultado;

            var disciplinasAgrupadas = uesPorNiveis.GroupBy(x => x.DisciplinaId);

            foreach (var grupoDisciplina in disciplinasAgrupadas)
            {
                var disciplinaPadrao = grupoDisciplina.FirstOrDefault();

                var disciplinaResumo = new DisciplinaResumoUesNivelProficienciaDto
                {
                    DisciplinaId = grupoDisciplina.Key,
                    DisciplinaNome = disciplinaPadrao.Disciplina,
                    UesPorNiveisProficiencia = grupoDisciplina
                        .GroupBy(x => x.NivelCodigo)
                        .Select(nivelGrupo => new NivelProficienciaUesDto
                        {
                            Codigo = nivelGrupo.Key,
                            Descricao = nivelGrupo.First().NivelDescricao,
                            QuantidadeUes = nivelGrupo.Count()
                        })
                        .OrderBy(x=> x.Codigo)
                        .ToList()
                };

                resultado.Disciplinas.Add(disciplinaResumo);
            }

            return resultado;
        }
    }
}

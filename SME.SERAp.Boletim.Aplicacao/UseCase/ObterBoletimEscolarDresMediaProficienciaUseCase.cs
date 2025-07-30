using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarDresMediaProficienciaUseCase : IObterBoletimEscolarDresMediaProficienciaUseCase
    {
        private readonly IMediator mediator;
        public ObterBoletimEscolarDresMediaProficienciaUseCase(IMediator mediator = null)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<BoletimEscolarDreMediaProficienciaDto>> Executar(long loteId, int anoEscolar, IEnumerable<long> dresIds)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            var dresMediaProficiencia = await mediator
                .Send(new ObterDresMediaProficienciaPorDisciplinaQuery(loteId, anoEscolar, dresIds));

            if (!dresMediaProficiencia?.Any() ?? true)
                return new List<BoletimEscolarDreMediaProficienciaDto>();

            var boletimEscolarDresMediaProficiencia = dresMediaProficiencia
                .GroupBy(d => new { d.DreId, d.DreNome })
                .Select(dre => new BoletimEscolarDreMediaProficienciaDto
                {
                    DreId = dre.FirstOrDefault().DreId,
                    DreNome = dre.FirstOrDefault().DreNome.RemoverPrefixoDre().Replace('/', ' '),
                    Diciplinas = dre.Select(d => new DisciplinaMediaProficienciaDto
                    {
                        DisciplinaId = d.DisciplinaId,
                        Disciplina = d.Disciplina,
                        MediaProficiencia = d.MediaProficiencia
                    })
                });

            return boletimEscolarDresMediaProficiencia;
        }
    }
}

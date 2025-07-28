using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficiencia;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDresPorNivelProficienciaDisciplinaUseCase : IObterDresPorNivelProficienciaDisciplinaUseCase
    {
        private readonly IMediator mediator;
        public ObterDresPorNivelProficienciaDisciplinaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ResumoDresNivelProficienciaDto> Executar(int anoEscolar, long loteId)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            var resultado = new ResumoDresNivelProficienciaDto
            {
                AnoEscolar = anoEscolar,
                LoteId = loteId,
                Disciplinas = new List<DisciplinaResumoDresNivelProficienciaDto>()
            };

            var dresPorNiveis = await mediator.Send(new ObterNiveisProficienciaQuery(anoEscolar, loteId));


            if (dresPorNiveis == null || !dresPorNiveis.Any())
                return resultado;

            var disciplinasAgrupadas = dresPorNiveis.GroupBy(x => x.DisciplinaId);

            foreach (var grupoDisciplina in disciplinasAgrupadas)
            {
                var disciplinaPadrao = grupoDisciplina.FirstOrDefault();

                var disciplinaResumo = new DisciplinaResumoDresNivelProficienciaDto
                {
                    DisciplinaId = grupoDisciplina.Key,
                    DisciplinaNome = disciplinaPadrao.Disciplina,
                    DresPorNiveisProficiencia = grupoDisciplina
                        .GroupBy(x => x.NivelCodigo)
                        .Select(nivelGrupo => new NivelProficienciaDresDto
                        {
                            Codigo = nivelGrupo.Key,
                            Descricao = nivelGrupo.First().NivelDescricao,
                            QuantidadeDres = nivelGrupo.Count()
                        })
                        .OrderBy(x => x.Codigo)
                        .ToList()
                };

                resultado.Disciplinas.Add(disciplinaResumo);
            }

            return resultado;
        }
    }
}
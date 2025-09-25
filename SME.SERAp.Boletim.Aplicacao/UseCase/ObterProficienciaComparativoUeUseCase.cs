using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoUe;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoUeUseCase : IObterProficienciaComparativoUeUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaComparativoUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ProficienciaComparativoUeDto> Executar(int dreId, int disciplinaId, int anoLetivo, int anoEscolar, int? ueId, List<int>? tiposVariacao, string? nomeUe, int? pagina, int? itensPorPagina)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            return await mediator.Send(new ObterProficienciaComparativoUeQuery(
                dreId,
                disciplinaId,
                anoLetivo,
                anoEscolar,
                ueId,
                tiposVariacao,
                nomeUe,
                pagina,
                itensPorPagina
            ));
        }
    }
}
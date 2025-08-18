using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaDreUseCase : IObterProficienciaDreUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ProficienciaDreCompletoDto> Executar(int anoEscolar, long loteId, IEnumerable<long> dreIds = null)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            return await mediator.Send(new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds));
        }
    }
}
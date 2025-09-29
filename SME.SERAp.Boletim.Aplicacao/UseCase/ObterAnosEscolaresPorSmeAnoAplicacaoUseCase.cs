using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnosEscolaresPorSmeAnoAplicacao;
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
    public class ObterAnosEscolaresPorSmeAnoAplicacaoUseCase : IObterAnosEscolaresPorSmeAnoAplicacaoUseCase
    {
        private readonly IMediator mediator;

        public ObterAnosEscolaresPorSmeAnoAplicacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Executar(int anoAplicacao, int disciplinaId)
        {
            var tipoPerfilUsuarioLogado = await mediator.Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            return await mediator.Send(new ObterAnosEscolaresPorSmeAnoAplicacaoQuery(anoAplicacao, disciplinaId));
        }
    }
}
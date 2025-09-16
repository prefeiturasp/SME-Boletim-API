using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoAlunoSpUseCase : IObterProficienciaComparativoAlunoSpUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaComparativoAlunoSpUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ProficienciaComparativoAlunoSpDto> Executar(int ueId, int disciplinaId, int anoEscolar, string turma, long loteId, int? tipoVariacao, int? pagina, int? itensPorPagina)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            return await mediator.Send(new ObterProficienciaComparativoAlunoSpQuery(ueId, disciplinaId, anoEscolar, turma, loteId, tipoVariacao, pagina, itensPorPagina));
        }
    }
}
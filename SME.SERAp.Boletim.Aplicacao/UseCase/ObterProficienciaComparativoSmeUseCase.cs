using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaSmeProvaSP;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoSmeUseCase : IObterProficienciaComparativoSmeUseCase
    {
        private readonly IMediator mediator;
        public ObterProficienciaComparativoSmeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TabelaComparativaSmePspPsaDto> Executar(int anoLetivo, int disciplinaId, int anoEscolar)
        {
            var tipoPerfilUsuarioLogado = await mediator.Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            var proficienciasPsa = await mediator.Send(new ObterProficienciaSmeProvaSaberesQuery(anoLetivo, disciplinaId, anoEscolar));
            var listaProficienciasPsp = await mediator.Send(new ObterProficienciaSmeProvaSPQuery(anoLetivo - 1, disciplinaId, anoEscolar - 1));
            var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdQuery(disciplinaId, anoEscolar));

            var listaProdificiencasComparativaSme = new List<ProficienciaTabelaComparativaSmeDto>();

            var proficienciaComparativaPspSmeDto = new ProficienciaTabelaComparativaSmeDto();
            var proficienciasPsp = listaProficienciasPsp?.FirstOrDefault();

            if (proficienciasPsp is not null)
            {
                proficienciaComparativaPspSmeDto.Descricao = proficienciasPsp.NomeAplicacao;
                proficienciaComparativaPspSmeDto.Mes = proficienciasPsp.Periodo;
                proficienciaComparativaPspSmeDto.ValorProficiencia = Math.Round((decimal)proficienciasPsp?.MediaProficiencia, 2);
                proficienciaComparativaPspSmeDto.NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficienciasPsp?.MediaProficiencia, disciplinaId, niveisProficiencia));
                proficienciaComparativaPspSmeDto.QtdeEstudante = proficienciasPsp.RealizaramProva;
                proficienciaComparativaPspSmeDto.QtdeUe = proficienciasPsp.QuantidadeUes;
                proficienciaComparativaPspSmeDto.QtdeDre = proficienciasPsp.QuantidadeDres;
                listaProdificiencasComparativaSme.Add(proficienciaComparativaPspSmeDto);
            }

            if (proficienciasPsa.Any())
            {
                foreach (var proficiencia in proficienciasPsa)
                {
                    var proficienciaComparativaPsaDreDto = new ProficienciaTabelaComparativaSmeDto();

                    proficienciaComparativaPsaDreDto.Descricao = proficiencia.NomeAplicacao;
                    proficienciaComparativaPsaDreDto.Mes = proficiencia.Periodo;
                    proficienciaComparativaPsaDreDto.ValorProficiencia = Math.Round((decimal)proficiencia.MediaProficiencia, 2);
                    proficienciaComparativaPsaDreDto.NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)proficiencia.MediaProficiencia, disciplinaId, niveisProficiencia));
                    proficienciaComparativaPsaDreDto.QtdeEstudante = proficiencia.RealizaramProva;
                    proficienciaComparativaPsaDreDto.QtdeUe = proficiencia.QuantidadeUes;
                    proficienciaComparativaPsaDreDto.QtdeDre = proficiencia.QuantidadeDres;
                    listaProdificiencasComparativaSme.Add(proficienciaComparativaPsaDreDto);
                }
            }

            decimal variacao = calculaVariacao(proficienciasPsa, proficienciasPsp);

            var tabelaRetorno = new TabelaComparativaSmePspPsaDto();
            tabelaRetorno.Variacao = variacao;
            tabelaRetorno.Aplicacao = listaProdificiencasComparativaSme;
            return tabelaRetorno;
        }

        private static decimal calculaVariacao(IEnumerable<ResultadoProeficienciaSme> proficienciasPsa, ResultadoProeficienciaSme proficienciasPsp)
        {
            var proficienciaFinal = proficienciasPsa.LastOrDefault() != null ? proficienciasPsa.LastOrDefault().MediaProficiencia : 0;
            var mediaProficiencia = proficienciasPsp != null ? proficienciasPsp.MediaProficiencia : 0;
            var variacao = (decimal)BoletimExtensions.CalcularPercentual((decimal)proficienciaFinal, (decimal)mediaProficiencia);
            return variacao;
        }
    }
}

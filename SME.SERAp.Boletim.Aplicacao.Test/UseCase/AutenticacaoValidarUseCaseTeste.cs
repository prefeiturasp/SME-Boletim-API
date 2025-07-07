using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.RemoverCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class AutenticacaoValidarUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Token_Quando_Codigo_Valido_E_Abrangencia_Existe()
        {
            var codigoValidacao = "COD123";

            var abrangencias = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto("usuario", Guid.NewGuid())
                {
                    DreId = 1,
                    UeId = 2,
                    TurmaId = 3,
                    Inicio = DateTime.Now,
                    Fim = DateTime.Now.AddMonths(1)
                }
            };

            var tokenEsperado = new AutenticacaoRetornoDto(
                token: "token123",
                dataHoraExpiracao: DateTime.UtcNow.AddHours(1),
                tipoPerfil: TipoPerfil.Professor
            );

            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.Is<ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery>(q => q.Codigo == codigoValidacao), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(abrangencias.AsEnumerable());

            mediator.Setup(m => m.Send(It.Is<RemoverCodigoValidacaoAutenticacaoCommand>(cmd => cmd.Codigo == codigoValidacao), It.IsAny<CancellationToken>()))
             .ReturnsAsync(true);


            mediator.Setup(m => m.Send(It.Is<ObterTokenJwtQuery>(q => q.Abrangencias == abrangencias), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(tokenEsperado);

            var useCase = new AutenticacaoValidarUseCase(mediator.Object);

            var resultado = await useCase.Executar(new AutenticacaoValidarDto(codigoValidacao));

            Assert.NotNull(resultado);
            Assert.Equal(tokenEsperado.Token, resultado.Token);
            Assert.Equal(tokenEsperado.DataHoraExpiracao, resultado.DataHoraExpiracao);
            Assert.Equal(tokenEsperado.TipoPerfil, resultado.TipoPerfil);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Codigo_Invalido()
        {
            var codigoValidacao = "COD_INVALIDO";

            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Enumerable.Empty<AbrangenciaDetalheDto>());

            var useCase = new AutenticacaoValidarUseCase(mediator.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(new AutenticacaoValidarDto(codigoValidacao)));

            Assert.Equal(401, ex.StatusCode);
            Assert.Equal("Código inválido", ex.Message);
        }
    }
}

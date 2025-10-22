using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Exceptions;


namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class AutenticacaoUseCaseTeste
    {
        private const string NomeVariavelAmbiente = "ChaveSerapProvaApi";

        [Fact]
        public async Task Executar_Deve_Retornar_Resultado_Quando_Chave_Valida_E_Abrangencia_Existe()
        {
            var chaveApi = "chave-teste";
            Environment.SetEnvironmentVariable(NomeVariavelAmbiente, chaveApi);

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

            var autenticacaoDto = new AutenticacaoDto
            {
                ChaveApi = chaveApi,
                Login = "usuario",
                Perfil = abrangencias[0].Perfil
            };

            var resultadoEsperado = new AutenticacaoValidarDto("XYZ123");

            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginGrupoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(abrangencias.AsEnumerable());

            mediator.Setup(m => m.Send(It.IsAny<GerarCodigoValidacaoAutenticacaoCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(resultadoEsperado);

            var useCase = new AutenticacaoUseCase(mediator.Object);

            var resultado = await useCase.Executar(autenticacaoDto);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Codigo, resultado.Codigo);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Resultado_Quando_VariavelAmbiente_Nao_Definida()
        {
            Environment.SetEnvironmentVariable(NomeVariavelAmbiente, null);

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

            var autenticacaoDto = new AutenticacaoDto
            {
                ChaveApi = null,
                Login = "usuario",
                Perfil = abrangencias[0].Perfil
            };

            var resultadoEsperado = new AutenticacaoValidarDto("XYZ123");

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginGrupoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(abrangencias.AsEnumerable());

            mediator.Setup(m => m.Send(It.IsAny<GerarCodigoValidacaoAutenticacaoCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(resultadoEsperado);

            var useCase = new AutenticacaoUseCase(mediator.Object);

            var resultado = await useCase.Executar(autenticacaoDto);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Codigo, resultado.Codigo);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_ChaveApi_Nao_Informada()
        {
            var chaveApi = "chave-teste";
            Environment.SetEnvironmentVariable(NomeVariavelAmbiente, chaveApi);

            var autenticacaoDto = new AutenticacaoDto
            {
                ChaveApi = null,
                Login = "usuario",
                Perfil = Perfis.PERFIL_PROFESSOR
            };

            var mediator = new Mock<IMediator>();
            var useCase = new AutenticacaoUseCase(mediator.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(autenticacaoDto));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_ChaveApi_Diferente()
        {
            Environment.SetEnvironmentVariable(NomeVariavelAmbiente, "chave-teste");

            var autenticacaoDto = new AutenticacaoDto
            {
                ChaveApi = "outra-chave",
                Login = "usuario",
                Perfil = Perfis.PERFIL_PROFESSOR
            };

            var mediator = new Mock<IMediator>();
            var useCase = new AutenticacaoUseCase(mediator.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(autenticacaoDto));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Nao_Existe_Abrangencia()
        {
            var chaveApi = "chave-teste";
            Environment.SetEnvironmentVariable(NomeVariavelAmbiente, chaveApi);

            var autenticacaoDto = new AutenticacaoDto
            {
                ChaveApi = chaveApi,
                Login = "usuario",
                Perfil = Perfis.PERFIL_PROFESSOR
            };

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginGrupoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<AbrangenciaDetalheDto>()); // vazio

            var useCase = new AutenticacaoUseCase(mediator.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(autenticacaoDto));
        }


    }
}

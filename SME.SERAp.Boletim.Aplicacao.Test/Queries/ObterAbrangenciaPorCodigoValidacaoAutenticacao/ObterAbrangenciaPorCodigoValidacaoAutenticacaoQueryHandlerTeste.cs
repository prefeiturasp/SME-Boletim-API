using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Cache;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioCache> repositorioCacheMock;
        private readonly ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandler handler;

        public ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandlerTeste()
        {
            repositorioCacheMock = new Mock<IRepositorioCache>();
            handler = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQueryHandler(repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Abrangencias_Quando_Existir_No_Cache()
        {
            var codigo = "ABC123";
            var chave = string.Format(CacheChave.Autenticacao, codigo);
            var abrangenciasEsperadas = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto { Id = 1, UsuarioId = 10, Login = "user1", Usuario = "Usuário 1", GrupoId = 100, Perfil = System.Guid.NewGuid(), Grupo = "Grupo A", DreId = 200, UeId = 300, TurmaId = 400, Inicio = System.DateTime.Now, Fim = System.DateTime.Now.AddDays(1) }
            };

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave))
                .ReturnsAsync(abrangenciasEsperadas);

            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(codigo);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(abrangenciasEsperadas[0].Id, resultado.First().Id);

            repositorioCacheMock.Verify(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Nao_Existir_Abrangencias()
        {
            var codigo = "XYZ999";
            var chave = string.Format(CacheChave.Autenticacao, codigo);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave))
                .ReturnsAsync((List<AbrangenciaDetalheDto>)null);

            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(codigo);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.Null(resultado);

            repositorioCacheMock.Verify(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Abrangencias_Estiverem_Vazias()
        {
            var codigo = "SEMRESULTADO";
            var chave = string.Format(CacheChave.Autenticacao, codigo);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave))
                .ReturnsAsync(new List<AbrangenciaDetalheDto>());

            var query = new ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(codigo);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.Null(resultado);

            repositorioCacheMock.Verify(r => r.ObterRedisAsync<List<AbrangenciaDetalheDto>>(chave), Times.Once);
        }
    }
}
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Cache;


namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAlunoSerapPorRaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioAluno> repositorioAlunoMock;
        private readonly Mock<IRepositorioCache> repositorioCacheMock;
        private readonly ObterAlunoSerapPorRaQueryHandler handler;

        public ObterAlunoSerapPorRaQueryHandlerTeste()
        {
            repositorioAlunoMock = new Mock<IRepositorioAluno>();
            repositorioCacheMock = new Mock<IRepositorioCache>();
            handler = new ObterAlunoSerapPorRaQueryHandler(repositorioAlunoMock.Object, repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Aluno_Do_Cache_Quando_Existir()
        {
            var alunoRa = 123456;
            var alunoEsperado = new Aluno("João", alunoRa) { Sexo = "M", TurmaId = 1, Situacao = 1 };
            var chave = string.Format(CacheChave.AlunoRa, alunoRa);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()))
                .ReturnsAsync(alunoEsperado);

            var query = new ObterAlunoSerapPorRaQuery(alunoRa);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(alunoEsperado.RA, resultado.RA);
            Assert.Equal(alunoEsperado.Nome, resultado.Nome);
            Assert.Equal(alunoEsperado.Sexo, resultado.Sexo);

            repositorioCacheMock.Verify(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()), Times.Once);
            repositorioAlunoMock.Verify(r => r.ObterPorRA(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Aluno_Do_Repositorio_Quando_Nao_Existir_No_Cache()
        {

            var alunoRa = 654321;
            var alunoEsperado = new Aluno("Maria", alunoRa) { Sexo = "F", TurmaId = 2, Situacao = 1 };
            var chave = string.Format(CacheChave.AlunoRa, alunoRa);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()))
                .ReturnsAsync((Aluno)null);

            repositorioAlunoMock
                .Setup(r => r.ObterPorRA(alunoRa))
                .ReturnsAsync(alunoEsperado);

            repositorioCacheMock
        .Setup(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()))
        .Returns<string, Func<Task<Aluno>>, int>((_, buscarDados, __) => buscarDados());

            var query = new ObterAlunoSerapPorRaQuery(alunoRa);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(alunoEsperado.RA, resultado.RA);
            Assert.Equal(alunoEsperado.Nome, resultado.Nome);
            Assert.Equal(alunoEsperado.Sexo, resultado.Sexo);

            repositorioAlunoMock.Verify(r => r.ObterPorRA(alunoRa), Times.Once);
                repositorioCacheMock.Verify(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()), Times.Exactly(1));

        }

        [Fact]
        public async Task Deve_Retornar_Nulo_Quando_Aluno_Nao_Existir()
        {
            var alunoRa = 999999;
            var chave = string.Format(CacheChave.AlunoRa, alunoRa);

            repositorioCacheMock
                .Setup(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()))
                .ReturnsAsync((Aluno)null);

            repositorioAlunoMock
                .Setup(r => r.ObterPorRA(alunoRa))
                .ReturnsAsync((Aluno)null);

            repositorioCacheMock
             .Setup(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()))
             .Returns<string, Func<Task<Aluno>>, int>((_, buscarDados, __) => buscarDados());

            var query = new ObterAlunoSerapPorRaQuery(alunoRa);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);

            repositorioAlunoMock.Verify(r => r.ObterPorRA(alunoRa), Times.Once);
            repositorioCacheMock.Verify(r => r.ObterRedisAsync<Aluno>(chave, It.IsAny<Func<Task<Aluno>>>(), It.IsAny<int>()), Times.Exactly(1));
        }
    }
}
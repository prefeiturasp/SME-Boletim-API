using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Aluno;
using SME.SERAp.Boletim.Infra.Dtos.Autenticacao;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;
using Xunit;
using SME.SERAp.Boletim.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios.Serap
{
    internal class RepositorioAlunoFake : RepositorioAluno
    {
        private readonly IDbConnection _conexaoLeitura;

        public RepositorioAlunoFake(ConnectionStringOptions options, IDbConnection conexaoLeitura) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
    }

    public class RepositorioAlunoTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly RepositorioAlunoFake repositorio;

        public RepositorioAlunoTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            repositorio = new RepositorioAlunoFake(new ConnectionStringOptions(), conexaoLeitura.Object);
        }

        [Fact]
        public async Task Deve_Obter_Aluno_Por_RA()
        {
            var ra = 123456789;
            var alunoMock = new Aluno { RA = ra, Nome = "João da Silva", Situacao = 1 };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Aluno>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(alunoMock);

            var resultado = await repositorio.ObterPorRA(ra);

            Assert.NotNull(resultado);
            Assert.Equal(ra, resultado.RA);
            Assert.Equal("João da Silva", resultado.Nome);
        }

        [Fact]
        public async Task Deve_Obter_Detalhe_Do_Aluno_Por_RA()
        {
            var ra = 987654321;
            var alunoDetalheMock = new AlunoDetalheDto
            {
                AlunoId = 1,
                Nome = "Maria",
                DreAbreviacao = "DRE B",
                Escola = "Escola C",
                Turma = "4º Ano A"
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<AlunoDetalheDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(alunoDetalheMock);

            var resultado = await repositorio.ObterAlunoDetalhePorRa(ra);

            Assert.NotNull(resultado);
            Assert.Equal("Maria", resultado.Nome);
            Assert.Equal("DRE B", resultado.DreAbreviacao);
        }

        [Fact]
        public async Task Deve_Obter_Aluno_Ativo_Por_RA()
        {
            var ra = 112233445;
            var alunoAtivoMock = new ObterAlunoAtivoRetornoDto
            {
                Ra = ra,
                Ano = "5",
                TurmaId = 55,
                Modalidade = Modalidade.Fundamental,
                TipoTurno = 2
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<ObterAlunoAtivoRetornoDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(alunoAtivoMock);

            var resultado = await repositorio.ObterAlunoAtivoPorRa(ra);

            Assert.NotNull(resultado);
            Assert.Equal(ra, resultado.Ra);
            Assert.Equal("5", resultado.Ano);
        }
    }
}
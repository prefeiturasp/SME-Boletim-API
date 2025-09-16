using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaComparativoAlunoSp
{
    public class ObterProficienciaComparativoAlunoSpHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolar;
        private readonly Mock<IMediator> mediator;
        private readonly ObterProficienciaComparativoAlunoSpHandler handler;

        public ObterProficienciaComparativoAlunoSpHandlerTeste()
        {
            repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            mediator = new Mock<IMediator>();
            handler = new ObterProficienciaComparativoAlunoSpHandler(repositorioBoletimEscolar.Object, mediator.Object);
        }

        private ObterProficienciaComparativoAlunoSpQuery ObterQuery()
        {
            return new ObterProficienciaComparativoAlunoSpQuery(
                ueId: 1,
                disciplinaId: 10,
                anoEscolar: 8,
                turma: "8A",
                anoCriacao: 2025,
                pagina: 1,
                itensPorPagina: 10
            );
        }

        private IEnumerable<AlunoProficienciaDto> ObterProficienciasAnoCorrente()
        {
            return new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 250, Periodo = "Abril", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 101, NomeAluno = "Maria", Proficiencia = 300, Periodo = "Abril", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 102, NomeAluno = "Pedro", Proficiencia = 280, Periodo = "Abril", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 275, Periodo = "Junho", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 103, NomeAluno = "Ana", Proficiencia = 320, Periodo = "Abril", NomeAplicacao = "P. Saberes" },
            };
        }

        private IEnumerable<AlunoProficienciaDto> ObterProficienciasAnoAnterior()
        {
            return new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 240 },
                new AlunoProficienciaDto { AlunoRa = 101, NomeAluno = "Maria", Proficiencia = 290 },
            };
        }

        [Fact(DisplayName = "Deve retornar dados paginados e ordenados quando todos os dados estão presentes")]
        public async Task Deve_Retornar_Dados_Completos_E_Paginados()
        {
            var query = ObterQuery();
            var proficienciasCorrente = ObterProficienciasAnoCorrente().ToList();
            var proficienciasAnterior = ObterProficienciasAnoAnterior().ToList();
            var anoLetivoPSP = query.AnoCriacao - 1;
            var alunosRa = proficienciasCorrente.Select(x => x.AlunoRa).Distinct().ToList();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaAlunoProvaSPAsync(query.DisciplinaId, anoLetivoPSP, It.Is<List<long>>(list => list.All(alunosRa.Contains) && list.Count == alunosRa.Count)))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(proficienciasCorrente.Select(x => x.AlunoRa).Distinct().Count(), resultado.Total);
            Assert.Equal(query.Pagina, resultado.Pagina);
            Assert.Equal(query.ItensPorPagina, resultado.ItensPorPagina);
            Assert.Equal(4, resultado.Itens.Count());
            Assert.Equal("Ana", resultado.Itens.First().Nome);
            Assert.Equal("Pedro", resultado.Itens.Last().Nome);

            var joao = resultado.Itens.First(i => i.Nome == "João");
            Assert.Equal(3, joao.Proficiencias.Count());
            Assert.Equal("PSP", joao.Proficiencias.First().Descricao);
            Assert.Equal(240, joao.Proficiencias.First().Valor);
            Assert.Equal(35, joao.Variacao);

            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSaberesAsync(query.UeId, query.DisciplinaId, query.AnoEscolar, query.Turma, query.AnoCriacao), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSPAsync(query.DisciplinaId, anoLetivoPSP, It.IsAny<List<long>>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando nenhuma proficiência é encontrada")]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nenhum_Dado_E_Encontrado()
        {
            var query = ObterQuery();
            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Itens);
            Assert.Empty(resultado.Aplicacoes);
            Assert.Equal(0, resultado.Total);

            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar dados com proficiência anterior nula se não existir")]
        public async Task Deve_Retornar_Dados_Com_Proficiencia_Anterior_Nula()
        {
            var query = ObterQuery();
            var proficienciasCorrente = ObterProficienciasAnoCorrente().ToList();
            var proficienciasAnterior = new List<AlunoProficienciaDto>();

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(4, resultado.Itens.Count());
            Assert.Equal("Ana", resultado.Itens.First().Nome);
            Assert.Equal(0, resultado.Itens.First(i => i.Nome == "Ana").Proficiencias.Count(p => p.Descricao == "PSP"));
            Assert.Equal(0, resultado.Itens.First(i => i.Nome == "Ana").Variacao);
        }

        [Fact(DisplayName = "Deve calcular a variação corretamente com a última proficiência do ano corrente")]
        public async Task Deve_Calcular_Variacao_Com_Ultima_Proficiencia()
        {
            var query = ObterQuery();
            var proficienciasCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 250, Periodo = "Abril", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 275.50m, Periodo = "Junho", NomeAplicacao = "P. Saberes" }
            };
            var proficienciasAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 240.25m }
            };

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            var joao = resultado.Itens.First();

            Assert.Equal(3, joao.Proficiencias.Count());
            Assert.Equal((double)(275.50m - 240.25m), joao.Variacao);
            Assert.Equal((double)35.25m, joao.Variacao);
        }

        [Fact(DisplayName = "Deve ordenar a lista de proficiências por período dentro de cada aluno")]
        public async Task Deve_Ordenar_Proficiencias_Por_Periodo()
        {
            var query = ObterQuery();
            var proficienciasCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 275, Periodo = "Junho", NomeAplicacao = "P. Saberes" },
                new AlunoProficienciaDto { AlunoRa = 100, NomeAluno = "João", Proficiencia = 250, Periodo = "Abril", NomeAplicacao = "P. Saberes" }
            };
            var proficienciasAnterior = ObterProficienciasAnoAnterior().ToList();

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            var joao = resultado.Itens.First();
            var proficienciasJoao = joao.Proficiencias.ToList();
            Assert.Equal("PSP", proficienciasJoao[0].Descricao);
            Assert.Equal("Abril", proficienciasJoao[1].Mes);
            Assert.Equal("Junho", proficienciasJoao[2].Mes);
        }

        [Fact(DisplayName = "Deve lidar com dados de repositório nulo sem lançar exceção")]
        public async Task Deve_Lidar_Com_Dados_Nulos_Sem_Lancar_Excecao()
        {
            var query = ObterQuery();
            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((IEnumerable<AlunoProficienciaDto>)null);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Itens);
            Assert.Empty(resultado.Aplicacoes);
            Assert.Equal(0, resultado.Total);

            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar proficiências ordenadas por nome do aluno")]
        public async Task Deve_Retornar_Proficiencias_Ordenadas_Por_Nome_Aluno()
        {
            var query = ObterQuery();
            var proficienciasCorrente = ObterProficienciasAnoCorrente().ToList();
            var proficienciasAnterior = ObterProficienciasAnoAnterior().ToList();

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar
                .Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            var nomesOrdenados = proficienciasCorrente.Select(p => p.NomeAluno).Distinct().OrderBy(n => n).ToList();
            var nomesResultado = resultado.Itens.Select(i => i.Nome).ToList();

            nomesResultado.Should().BeEquivalentTo(nomesOrdenados, opts => opts.WithStrictOrdering());
        }

        [Fact(DisplayName = "Deve mapear a lista de aplicações corretamente")]
        public async Task Deve_Mapear_Lista_De_Aplicacoes_Corretamente()
        {
            var query = ObterQuery();
            var proficienciasCorrente = ObterProficienciasAnoCorrente().ToList();
            var proficienciasAnterior = ObterProficienciasAnoAnterior().ToList();

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasCorrente);

            repositorioBoletimEscolar.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<long>>()))
                .ReturnsAsync(proficienciasAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            var aplicacoesEsperadas = new List<string> { "Abril", "Junho" };
            Assert.Equal(aplicacoesEsperadas, resultado.Aplicacoes);
        }
    }
}
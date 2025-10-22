using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaDre
{
    public class ObterProficienciaDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolar;
        private readonly ObterProficienciaDreQueryHandler queryHandler;

        public ObterProficienciaDreQueryHandlerTeste()
        {
            repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterProficienciaDreQueryHandler(repositorioBoletimEscolar.Object);
        }

        [Fact(DisplayName = "Deve retornar a lista de DREs com proficiência agrupada e contagem correta")]
        public async Task Deve_Retornar_Lista_Proficiencia_Agrupada_Corretamente()
        {
            var anoEscolar = 5;
            var loteId = 10L;
            var resumoDres = ObterDadosResumoDres();
            var mediasProficiencia = ObterDadosMediasProficiencia();
            var niveisProficiencia = ObterDadosNiveisProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Equal(2, resultado.Itens.Count());

            var primeiraDre = resultado.Itens.First(i => i.DreId == 1);
            Assert.Equal("DRE BUTANTA", primeiraDre.DreNome);
            Assert.Equal(2, primeiraDre.Disciplinas.Count());
            Assert.Equal("Língua portuguesa", primeiraDre.Disciplinas.First().Disciplina);
            Assert.Equal("Básico", primeiraDre.Disciplinas.First().NivelProficiencia);

            var segundaDre = resultado.Itens.First(i => i.DreId == 2);
            Assert.Equal("DRE ITAQUERA", segundaDre.DreNome);
            Assert.Single(segundaDre.Disciplinas);
            Assert.Equal("Matemática", segundaDre.Disciplinas.First().Disciplina);
            Assert.Equal("Básico", segundaDre.Disciplinas.First().NivelProficiencia);

            repositorioBoletimEscolar.Verify(r => r.ObterResumoDreAsync(anoEscolar, loteId), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterNiveisProficienciaAsync(anoEscolar), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar objeto com listas vazias se o resumo de DREs retornar vazio")]
        public async Task Deve_Retornar_Objeto_Com_Listas_Vazias_Se_Resumo_Retornar_Vazio()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(new List<DreResumoDto>());
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(ObterDadosMediasProficiencia());
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(ObterDadosNiveisProficiencia());

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
            repositorioBoletimEscolar.Verify(r => r.ObterResumoDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar objeto com listas vazias se o resumo de DREs retornar null")]
        public async Task Deve_Retornar_Objeto_Com_Listas_Vazias_Se_Resumo_Retornar_Null()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync((IEnumerable<DreResumoDto>)null);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(ObterDadosMediasProficiencia());
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(ObterDadosNiveisProficiencia());

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
            repositorioBoletimEscolar.Verify(r => r.ObterResumoDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve filtrar por uma única DreId corretamente")]
        public async Task Deve_Filtrar_Por_Uma_Unica_DreId_Corretamente()
        {
            var anoEscolar = 5;
            var loteId = 10L;
            var dreId = new List<long> { 1 };
            var resumoDres = ObterDadosResumoDres();
            var mediasProficiencia = ObterDadosMediasProficiencia();
            var niveisProficiencia = ObterDadosNiveisProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Single(resultado.Itens);
            Assert.Equal("DRE BUTANTA", resultado.Itens.First().DreNome);
            Assert.Equal(1, resultado.Itens.First().DreId);
        }

        [Fact(DisplayName = "Deve filtrar por múltiplas DreIds corretamente")]
        public async Task Deve_Filtrar_Por_Multiplas_DreIds_Corretamente()
        {
            var anoEscolar = 5;
            var loteId = 10L;
            var dreIds = new List<long> { 1, 2 };
            var resumoDres = ObterDadosResumoDres();
            var mediasProficiencia = ObterDadosMediasProficiencia();
            var niveisProficiencia = ObterDadosNiveisProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalTipoDisciplina);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Contains(resultado.Itens, i => i.DreNome == "DRE BUTANTA");
            Assert.Contains(resultado.Itens, i => i.DreNome == "DRE ITAQUERA");
        }

        [Fact(DisplayName = "Deve retornar lista vazia se nenhuma DreId do filtro existir")]
        public async Task Deve_Retornar_Lista_Vazia_Se_Nenhuma_DreId_Do_Filtro_Existir()
        {
            var anoEscolar = 5;
            var loteId = 10L;
            var dreIds = new List<long> { 99, 100 };
            var resumoDres = ObterDadosResumoDres();
            var mediasProficiencia = ObterDadosMediasProficiencia();
            var niveisProficiencia = ObterDadosNiveisProficiencia();

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
            Assert.Empty(resultado.Itens);
        }

        [Fact(DisplayName = "Deve retornar nível 'Avançado' se média maior que todos valores de referência")]
        public async Task Deve_Retornar_Nivel_Avancado()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE TESTE", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 }
            };

            var mediasProficiencia = new List<DreMediaProficienciaDto>
            {
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Matemática", DisciplinaId = 2, MediaProficiencia = 999 }
            };

            var niveisProficiencia = new List<DreNivelProficienciaDto>
            {
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Básico", ValorReferencia = 190 },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Adequado", ValorReferencia = 210 },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Avançado", ValorReferencia = null }
            };

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Equal("Avançado", resultado.Itens.First().Disciplinas.First().NivelProficiencia);
        }

        [Fact(DisplayName = "Deve retornar 'Nível não definido' se não houver nível sem referência")]
        public async Task Deve_Retornar_NivelNaoDefinido_Se_NaoHouverNivelSemReferencia()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE TESTE", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 }
            };

            var mediasProficiencia = new List<DreMediaProficienciaDto>
            {
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Matemática", DisciplinaId = 2, MediaProficiencia = 999 }
            };

            var niveisProficiencia = new List<DreNivelProficienciaDto>
            {
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Básico", ValorReferencia = 190 },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Adequado", ValorReferencia = 210 }
            };

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Equal("Nível não definido", resultado.Itens.First().Disciplinas.First().NivelProficiencia);
        }

        [Fact(DisplayName = "Deve retornar percentual de participação = 0 quando TotalAlunos = 0")]
        public async Task Deve_Retornar_PercentualParticipacao_Zero_Se_TotalAlunos_Zero()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE ZERO", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 0, TotalRealizaramProva = 0 }
            };

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(new List<DreMediaProficienciaDto>());
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(new List<DreNivelProficienciaDto>());

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Equal(0, resultado.Itens.First().PercentualParticipacao);
        }

        [Fact(DisplayName = "Deve retornar disciplinas vazias se nenhuma média existir para a DRE")]
        public async Task Deve_Retornar_Disciplinas_Vazias_Se_Nenhuma_Media_Existir()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE SEM MEDIA", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 }
            };

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(new List<DreMediaProficienciaDto>());
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(new List<DreNivelProficienciaDto>());

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Empty(resultado.Itens.First().Disciplinas);
            Assert.Equal(0, resultado.TotalTipoDisciplina);
        }

        [Fact(DisplayName = "Deve ordenar itens por DreNome")]
        public async Task Deve_Ordenar_Itens_Por_DreNome()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 2, DreNome = "Z DRE", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 },
                new DreResumoDto { DreId = 1, DreNome = "A DRE", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 }
            };

            var mediasProficiencia = new List<DreMediaProficienciaDto>();
            var niveisProficiencia = new List<DreNivelProficienciaDto>();

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Equal("A DRE", resultado.Itens.FirstOrDefault()?.DreNome);
            Assert.Equal("Z DRE", resultado.Itens.LastOrDefault()?.DreNome);
        }

        [Fact(DisplayName = "Deve retornar 'Nível não definido' se disciplina não existir nos níveis")]
        public async Task Deve_Retornar_NivelNaoDefinido_Se_Disciplina_Inexistente()
        {
            var anoEscolar = 5;
            var loteId = 10L;

            var resumoDres = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE TESTE", AnoEscolar = 5, TotalUes = 1, TotalAlunos = 10, TotalRealizaramProva = 5 }
            };

            var mediasProficiencia = new List<DreMediaProficienciaDto>
            {
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Geografia", DisciplinaId = 99, MediaProficiencia = 100 }
            };

            var niveisProficiencia = new List<DreNivelProficienciaDto>(); // Nenhum nível definido

            repositorioBoletimEscolar
                .Setup(r => r.ObterResumoDreAsync(anoEscolar, loteId))
                .ReturnsAsync(resumoDres);
            repositorioBoletimEscolar
                .Setup(r => r.ObterMediaProficienciaDreAsync(anoEscolar, loteId))
                .ReturnsAsync(mediasProficiencia);
            repositorioBoletimEscolar
                .Setup(r => r.ObterNiveisProficienciaAsync(anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            var query = new ObterProficienciaDreQuery(anoEscolar, loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Equal("Nível não definido", resultado.Itens.First().Disciplinas.First().NivelProficiencia);
        }

        private List<DreResumoDto> ObterDadosResumoDres()
        {
            return new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 1, DreNome = "DRE BUTANTA", AnoEscolar = 5, TotalUes = 29, TotalAlunos = 206, TotalRealizaramProva = 101 },
                new DreResumoDto { DreId = 2, DreNome = "DRE ITAQUERA", AnoEscolar = 5, TotalUes = 15, TotalAlunos = 150, TotalRealizaramProva = 75 }
            };
        }

        private List<DreMediaProficienciaDto> ObterDadosMediasProficiencia()
        {
            return new List<DreMediaProficienciaDto>
            {
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Língua portuguesa", DisciplinaId = 1, MediaProficiencia = 193.31m },
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Matemática", DisciplinaId = 2, MediaProficiencia = 176.67m },
                new DreMediaProficienciaDto { DreId = 2, Disciplina = "Matemática", DisciplinaId = 2, MediaProficiencia = 180.00m }
            };
        }

        private List<DreNivelProficienciaDto> ObterDadosNiveisProficiencia()
        {
            return new List<DreNivelProficienciaDto>
            {
                new DreNivelProficienciaDto { DisciplinaId = 1, Ano = 5, Descricao = "Avançado", ValorReferencia = null },
                new DreNivelProficienciaDto { DisciplinaId = 1, Ano = 5, Descricao = "Adequado", ValorReferencia = 220 },
                new DreNivelProficienciaDto { DisciplinaId = 1, Ano = 5, Descricao = "Básico", ValorReferencia = 200 },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Avançado", ValorReferencia = null },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Adequado", ValorReferencia = 210 },
                new DreNivelProficienciaDto { DisciplinaId = 2, Ano = 5, Descricao = "Básico", ValorReferencia = 190 }
            };
        }
    }
}
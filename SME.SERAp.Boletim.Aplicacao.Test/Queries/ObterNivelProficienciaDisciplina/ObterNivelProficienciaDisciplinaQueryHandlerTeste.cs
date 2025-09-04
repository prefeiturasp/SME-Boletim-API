using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterNivelProficienciaDisciplina
{
    public class ObterNivelProficienciaDisciplinaQueryHandlerTeste
    {
        private readonly ObterNivelProficienciaDisciplinaQueryHandler handler;

        public ObterNivelProficienciaDisciplinaQueryHandlerTeste()
        {
            handler = new ObterNivelProficienciaDisciplinaQueryHandler();
        }

        [Fact]
        public async Task Handle_DeveRetornarDescricaoCorreta_QuandoMediaCorrespondeANivel()
        {
            var disciplinaId = 1L;
            var media = 550.0M;
            var niveis = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Básico", ValorReferencia = 500 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Intermediário", ValorReferencia = 600 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Avançado", ValorReferencia = 700 },
            };
            var query = new ObterNivelProficienciaDisciplinaQuery(media, disciplinaId, niveis);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal("Nível Intermediário", resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarNivelCorreto_QuandoMediaIgualAoLimite()
        {
            var disciplinaId = 1L;
            var media = 600.0M;
            var niveis = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Básico", ValorReferencia = 500 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Intermediário", ValorReferencia = 600 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Avançado", ValorReferencia = 700 },
            };
            var query = new ObterNivelProficienciaDisciplinaQuery(media, disciplinaId, niveis);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal("Nível Intermediário", resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarNivelNaoDefinido_QuandoNaoHaNiveisParaADisciplina()
        {
            var disciplinaId = 2L;
            var media = 550.0M;
            var niveis = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = 1L, Ano = 2024, Descricao = "Nível Básico", ValorReferencia = 500 },
            };
            var query = new ObterNivelProficienciaDisciplinaQuery(media, disciplinaId, niveis);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal("Nível não definido", resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarNivelSemReferencia_QuandoMediaAcimaDeTodosOsNiveis()
        {
            var disciplinaId = 1L;
            var media = 800.0M;
            var niveis = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Básico", ValorReferencia = 500 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Intermediário", ValorReferencia = 600 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Avançado", ValorReferencia = 700 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = 2024, Descricao = "Nível Máximo", ValorReferencia = null },
            };
            var query = new ObterNivelProficienciaDisciplinaQuery(media, disciplinaId, niveis);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal("Nível Máximo", resultado);
        }
    }
}
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterDre
{
    public class ObterDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolar;
        private readonly ObterDreQueryHandler queryHandler;

        public ObterDreQueryHandlerTeste()
        {
            repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDreQueryHandler(repositorioBoletimEscolar.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de DREs ao executar handler com dados válidos")]
        public async Task Deve_Retornar_Lista_De_Dres()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;
            var dres = ObterDres();

            repositorioBoletimEscolar
                .Setup(r => r.ObterDreAsync(anoEscolar, loteId))
                .ReturnsAsync(dres);

            var query = new ObterDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dres.Count, resultado.Count());
            Assert.Equal(dres.First().DreId, resultado.First().DreId);
            repositorioBoletimEscolar.Verify(r => r.ObterDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia se repositório não retornar DREs")]
        public async Task Deve_Retornar_Lista_Vazia_Se_Repositorio_Nao_Trouxer_Dados()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterDreAsync(anoEscolar, loteId))
                .ReturnsAsync(new List<DreDto>());

            var query = new ObterDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            repositorioBoletimEscolar.Verify(r => r.ObterDreAsync(anoEscolar, loteId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar null se repositório retornar null")]
        public async Task Deve_Retornar_Null_Se_Repositorio_Retornar_Null()
        {
            // Arrange
            var anoEscolar = 5;
            var loteId = 10L;

            repositorioBoletimEscolar
                .Setup(r => r.ObterDreAsync(anoEscolar, loteId))
                .ReturnsAsync((IEnumerable<DreDto>)null);

            var query = new ObterDreQuery(anoEscolar, loteId);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
            repositorioBoletimEscolar.Verify(r => r.ObterDreAsync(anoEscolar, loteId), Times.Once);
        }

        private List<DreDto> ObterDres()
        {
            return new List<DreDto>
            {
                new DreDto
                {
                    DreId = 1,
                    DreNomeAbreviado = "DRE-CEU",
                    DreNome = "Diretoria CEU"
                },
                new DreDto
                {
                    DreId = 2,
                    DreNomeAbreviado = "DRE-SUL",
                    DreNome = "Diretoria Sul"
                }
            };
        }
    }
}
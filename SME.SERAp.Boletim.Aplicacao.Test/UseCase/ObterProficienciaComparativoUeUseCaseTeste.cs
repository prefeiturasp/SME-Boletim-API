using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoUe;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaComparativoUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly IObterProficienciaComparativoUeUseCase useCase;

        public ObterProficienciaComparativoUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProficienciaComparativoUeUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve chamar o Mediator com os parâmetros corretos e retornar o DTO")]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos_E_Retornar_Dto()
        {
            var dreId = 1;
            var disciplinaId = 10;
            var anoLetivo = 2024;
            var anoEscolar = 9;
            var ueId = 100;
            var tiposVariacao = new List<int> { 1, 2 };
            var nomeUe = "UE Teste";
            var pagina = 1;
            var itensPorPagina = 10;

            var proficienciaComparativoDto = new ProficienciaComparativoUeDto
            {
                Total = 1,
                Pagina = 1,
                ItensPorPagina = 10,
                DreId = dreId,
                DreAbreviacao = "DRE",
                Ues = new List<UeProficienciaDto>
                {
                    new UeProficienciaDto
                    {
                        UeId = ueId,
                        UeNome = nomeUe,
                        Disciplinaid = disciplinaId,
                        Variacao = 5.5,
                        AplicacaoPsp = new ProficienciaDetalheUeDto(),
                        AplicacoesPsa = new List<ProficienciaDetalheUeDto>()
                    }
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciaComparativoDto);

            var resultado = await useCase.Executar(
                dreId,
                disciplinaId,
                anoLetivo,
                anoEscolar,
                ueId,
                tiposVariacao,
                nomeUe,
                pagina,
                itensPorPagina
            );

            Assert.NotNull(resultado);
            Assert.IsType<ProficienciaComparativoUeDto>(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterProficienciaComparativoUeQuery>(
                    q => q.DreId == dreId &&
                         q.DisciplinaId == disciplinaId &&
                         q.AnoLetivo == anoLetivo &&
                         q.AnoEscolar == anoEscolar &&
                         q.UeId == ueId &&
                         q.TiposVariacao == tiposVariacao &&
                         q.NomeUe == nomeUe &&
                         q.Pagina == pagina &&
                         q.ItensPorPagina == itensPorPagina),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class TurmaAnoDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var ano = 2023;
            var turma = "Turma A";
            var descricao = "Descrição da Turma A";
            var dto = new TurmaAnoDto
            {
                Ano = ano,
                Turma = turma,
                Descricao = descricao
            };
            Assert.Equal(ano, dto.Ano);
            Assert.Equal(turma, dto.Turma);
            Assert.Equal(descricao, dto.Descricao);
        }
    }
}

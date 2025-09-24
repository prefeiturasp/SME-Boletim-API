using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class UeTotalAlunoDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var ueId = 1001L;
            var totalAlunos = 350;

            var dto = new UeTotalAlunoDto
            {
                UeId = ueId,
                TotalAlunos = totalAlunos
            };

            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(totalAlunos, dto.TotalAlunos);
        }

        [Fact]
        public void Deve_Permitir_TotalAlunos_Igual_A_Zero()
        {
            var ueId = 2002L;
            var totalAlunos = 0;

            var dto = new UeTotalAlunoDto
            {
                UeId = ueId,
                TotalAlunos = totalAlunos
            };

            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(0, dto.TotalAlunos);
        }
    }
}

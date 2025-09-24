using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Teste.Fila
{
    public class RotasRabbitTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Correto_Da_Constante()
        {
            Assert.Equal("ApplicationLog", RotasRabbit.RotaLogs);
        }
    }
}

using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Teste.Fila
{
    public class ExchangeRabbitTeste
    {
        [Fact]
        public void Deve_Retornar_Valores_Corretos_Das_Propriedades()
        {
            Assert.Equal("serap.estudante.workers", ExchangeRabbit.SerapEstudante);
            Assert.Equal("serap.workers", ExchangeRabbit.Serap);
            Assert.Equal("serap.estudante.acomp.workers", ExchangeRabbit.SerapEstudanteAcompanhamento);
            Assert.Equal("serap.estudante.workers.deadletter", ExchangeRabbit.SerapEstudanteDeadLetter);
            Assert.Equal("EnterpriseApplicationLog", ExchangeRabbit.Logs);
        }
    }
}

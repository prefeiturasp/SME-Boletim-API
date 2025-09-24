using SME.SERAp.Boletim.Infra.Extensions;
using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Teste.Fila
{
    public class MensagemRabbitTeste
    {
        private class MensagemRabbitFake : MensagemRabbit
        {
            public MensagemRabbitFake() : base()
            {
            }
        }

        private class TesteDto
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
        }

        [Fact]
        public void Deve_Permitir_Criar_Instancia_Com_Construtor_Protegido()
        {
            var instancia = new MensagemRabbitFake();

            Assert.NotNull(instancia);
            Assert.Null(instancia.Mensagem);
            Assert.Equal(Guid.Empty, instancia.CodigoCorrelacao);
        }

        [Fact]
        public void Deve_Criar_Objeto_Com_Propriedades_Corretas()
        {
            var codigo = Guid.NewGuid();
            var mensagem = new { Nome = "Joao", Idade = 30 };

            var obj = new MensagemRabbit(mensagem, codigo);

            Assert.Equal(mensagem, obj.Mensagem);
            Assert.Equal(codigo, obj.CodigoCorrelacao);
        }

        [Fact]
        public void ObterObjetoMensagem_Deve_Converter_Corretamente()
        {
            var codigo = Guid.NewGuid();
            var dto = new TesteDto { Nome = "Maria", Idade = 25 };
            var mensagemJson = dto.ConverterObjectParaJson();

            var obj = new MensagemRabbit(mensagemJson, codigo);

            var resultado = obj.ObterObjetoMensagem<TesteDto>();

            Assert.NotNull(resultado);
            Assert.Equal("Maria", resultado.Nome);
            Assert.Equal(25, resultado.Idade);
        }

        [Fact]
        public void ObterObjetoMensagem_Com_Mensagem_Nula_Deve_Retornar_Null()
        {
            var codigo = Guid.NewGuid();
            var obj = new MensagemRabbit(null, codigo);

            var resultado = obj.ObterObjetoMensagem<TesteDto>();

            Assert.Null(resultado);
        }
    }
}

using FluentValidation.Results;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos
{
    public class RetornoBaseDtoTeste
    {
        [Fact]
        public void Construtor_Com_ValidationFailures_Deve_Preencher_Mensagens()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro no campo 1"),
                new ValidationFailure("Campo2", "Erro no campo 2")
            };

            var dto = new RetornoBaseDto(failures);

            Assert.Equal(2, dto.Mensagens.Count);
            Assert.Contains("Erro no campo 1", dto.Mensagens);
            Assert.Contains("Erro no campo 2", dto.Mensagens);
            Assert.True(dto.ExistemErros);
        }

        [Fact]
        public void Construtor_Com_ValidationFailures_Vazio_Deve_Ter_Mensagens_Nulas()
        {
            var failures = new List<ValidationFailure>();

            var dto = new RetornoBaseDto(failures);

            Assert.Null(dto.Mensagens);
            Assert.False(dto.ExistemErros);
        }

        [Fact]
        public void Construtor_Sem_Parametros_Deve_Inicializar_Mensagens_Vazio()
        {
            var dto = new RetornoBaseDto();

            Assert.NotNull(dto.Mensagens);
            Assert.Empty(dto.Mensagens);
            Assert.False(dto.ExistemErros);
        }

        [Fact]
        public void Construtor_Com_Mensagem_Deve_Preencher_Lista()
        {
            var mensagem = "Erro genérico";

            var dto = new RetornoBaseDto(mensagem);

            Assert.Single(dto.Mensagens);
            Assert.Equal(mensagem, dto.Mensagens.First());
            Assert.True(dto.ExistemErros);
        }

        [Fact]
        public void ExistemErros_Deve_Retornar_False_Quando_Mensagens_For_Null()
        {
            var dto = new RetornoBaseDto { Mensagens = null };

            Assert.False(dto.ExistemErros);
        }
    }
}

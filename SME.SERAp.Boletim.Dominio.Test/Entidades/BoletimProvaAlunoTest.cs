using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Dominio.Enumerados;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class BoletimProvaAlunoTest
    {
        [Fact]
        public void Deve_Criar_BoletimProvaAluno_Com_Propriedades_Validas()
        {
            var boletim = new BoletimProvaAluno
            {
                DreId = 1,
                CodigoUe = "123456",
                NomeUe = "Escola Exemplo",
                ProvaId = 1001,
                ProvaDescricao = "Prova de Português",
                AnoEscolar = 5,
                Turma = "5A",
                AlunoRa = 1234567890,
                AlunoNome = "João da Silva",
                Disciplina = "Português",
                DisciplinaId = 2001,
                ProvaStatus = ProvaStatus.Finalizado,
                Proficiencia = 220.75m,
                ErroMedida = 5.5m,
                NivelCodigo = 3
            };

            Assert.Equal(1, boletim.DreId);
            Assert.Equal("123456", boletim.CodigoUe);
            Assert.Equal("Escola Exemplo", boletim.NomeUe);
            Assert.Equal(1001, boletim.ProvaId);
            Assert.Equal("Prova de Português", boletim.ProvaDescricao);
            Assert.Equal(5, boletim.AnoEscolar);
            Assert.Equal("5A", boletim.Turma);
            Assert.Equal(1234567890, boletim.AlunoRa);
            Assert.Equal("João da Silva", boletim.AlunoNome);
            Assert.Equal("Português", boletim.Disciplina);
            Assert.Equal(2001, boletim.DisciplinaId);
            Assert.Equal(ProvaStatus.Finalizado, boletim.ProvaStatus);
            Assert.Equal(220.75m, boletim.Proficiencia);
            Assert.Equal(5.5m, boletim.ErroMedida);
            Assert.Equal(3, boletim.NivelCodigo);
        }
    }
}

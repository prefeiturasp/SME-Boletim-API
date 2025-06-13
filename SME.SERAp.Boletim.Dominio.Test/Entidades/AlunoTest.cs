using System;
using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class AlunoTest
    {
        [Fact]
        public void Deve_Criar_Aluno_Com_Construtor_Padrao()
        {
            var aluno = new Aluno
            {
                Nome = "Maria",
                RA = 123456,
                Situacao = 1,
                TurmaId = 789,
                Sexo = "F",
                DataNascimento = new DateTime(2010, 5, 1),
                NomeSocial = "Mariazinha",
                DataAtualizacao = DateTime.Today
            };

            Assert.Equal("Maria", aluno.Nome);
            Assert.Equal(123456, aluno.RA);
            Assert.Equal(1, aluno.Situacao);
            Assert.Equal(789, aluno.TurmaId);
            Assert.Equal("F", aluno.Sexo);
            Assert.Equal(new DateTime(2010, 5, 1), aluno.DataNascimento);
            Assert.Equal("Mariazinha", aluno.NomeSocial);
            Assert.Equal(DateTime.Today, aluno.DataAtualizacao);
        }

        [Fact]
        public void Deve_Criar_Aluno_Com_Construtor_Parametros()
        {
            var aluno = new Aluno("João", 654321);

            Assert.Equal("João", aluno.Nome);
            Assert.Equal(654321, aluno.RA);
        }
    }
}

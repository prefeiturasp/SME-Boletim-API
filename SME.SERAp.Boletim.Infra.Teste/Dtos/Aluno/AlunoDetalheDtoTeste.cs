using SME.SERAp.Boletim.Infra.Dtos.Aluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.Aluno
{
    public class AlunoDetalheDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var alunoId = 123456L;
            var dreAbreviacao = "DRE-A";
            var escola = "Escola Municipal de Ensino Fundamental";
            var turma = "5º Ano A";
            var nome = "João da Silva";
            var nomeSocial = "Jô Silva";

            var dto = new AlunoDetalheDto
            {
                AlunoId = alunoId,
                DreAbreviacao = dreAbreviacao,
                Escola = escola,
                Turma = turma,
                Nome = nome,
                NomeSocial = nomeSocial
            };

            Assert.Equal(alunoId, dto.AlunoId);
            Assert.Equal(dreAbreviacao, dto.DreAbreviacao);
            Assert.Equal(escola, dto.Escola);
            Assert.Equal(turma, dto.Turma);
            Assert.Equal(nome, dto.Nome);
            Assert.Equal(nomeSocial, dto.NomeSocial);
        }

        [Fact]
        public void NomeFinal_DeveRetornarNomeSocial_QuandoPreenchido()
        {
            var dto = new AlunoDetalheDto
            {
                Nome = "Maria da Silva",
                NomeSocial = "Mary"
            };

            var nomeFinal = dto.NomeFinal();

            Assert.Equal("Mary", nomeFinal);
        }

        [Fact]
        public void NomeFinal_DeveRetornarNome_QuandoNomeSocialNaoPreenchido()
        {
            var dto = new AlunoDetalheDto
            {
                Nome = "José Carlos",
                NomeSocial = string.Empty
            };

            var nomeFinal = dto.NomeFinal();

            Assert.Equal("José Carlos", nomeFinal);
        }

        [Fact]
        public void NomeFinal_DeveRetornarNome_QuandoNomeSocialEhNulo()
        {
            var dto = new AlunoDetalheDto
            {
                Nome = "Ana Paula",
                NomeSocial = null
            };

            var nomeFinal = dto.NomeFinal();

            Assert.Equal("Ana Paula", nomeFinal);
        }
    }
}
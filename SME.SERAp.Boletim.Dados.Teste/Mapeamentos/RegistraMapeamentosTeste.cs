using Dapper.FluentMap;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Dados.Teste.Mapeamentos
{
    [Collection("ColecaoMapeamentos")]
    public class RegistraMapeamentosTeste
    {
        [Fact]
        public void Deve_Registrar_Todos_Os_Maps_Sem_Erro()
        {
            FluentMapper.EntityMaps.Clear();
            var ex = Record.Exception(() => RegistraMapeamentos.Registrar());

            Assert.Null(ex);
        }

        [Fact]
        public void Deve_Registrar_Todos_Os_Maps_Corretamente()
        {
            FluentMapper.EntityMaps.Clear();
            RegistraMapeamentos.Registrar();

            var tiposEsperados = new Type[]
            {
                typeof(Aluno),
                typeof(Abrangencia),
                typeof(BoletimEscolar),
                typeof(BoletimProvaAluno),
                typeof(BoletimResultadoProbabilidade),
            };

            foreach (var tipo in tiposEsperados)
            {
                Assert.Contains(tipo, FluentMapper.EntityMaps.Keys);
            }
        }
    }
}

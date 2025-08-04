using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;


namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterAbrangenciaPorLoginGrupoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangenciaMock;
        private readonly ObterAbrangenciaPorLoginGrupoQueryHandler handler;

        public ObterAbrangenciaPorLoginGrupoQueryHandlerTeste()
        {
            repositorioAbrangenciaMock = new Mock<IRepositorioAbrangencia>();
            handler = new ObterAbrangenciaPorLoginGrupoQueryHandler(repositorioAbrangenciaMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Abrangencia_Administrador()
        {
            var login = "admin";
            var perfilAdministrador = Guid.Parse("AAD9D772-41A3-E411-922D-782BCB3D218E");
            var query = new ObterAbrangenciaPorLoginGrupoQuery(login, perfilAdministrador);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(login, resultado.First().Login);
            Assert.Equal(perfilAdministrador, resultado.First().Perfil);

            repositorioAbrangenciaMock.Verify(r => r.ObterAbrangenciaPorLoginGrupo(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Abrangencia_Do_Repositorio_Quando_Nao_Administrador()
        {
            var login = "usuario";
            var perfil = Guid.Empty; 
            var abrangenciasEsperadas = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto(login, perfil)
            };

            repositorioAbrangenciaMock
                .Setup(r => r.ObterAbrangenciaPorLoginGrupo(login, perfil))
                .ReturnsAsync(abrangenciasEsperadas);

            var query = new ObterAbrangenciaPorLoginGrupoQuery(login, perfil);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(login, resultado.First().Login);
            Assert.Equal(perfil, resultado.First().Perfil);

            repositorioAbrangenciaMock.Verify(r => r.ObterAbrangenciaPorLoginGrupo(login, perfil), Times.Once);
        }
    }
}
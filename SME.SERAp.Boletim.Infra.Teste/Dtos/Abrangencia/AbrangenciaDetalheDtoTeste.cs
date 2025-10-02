using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.Abrangencia
{
    public class AbrangenciaDetalheDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var id = 1L;
            var usuarioId = 10L;
            var login = "login.teste";
            var usuario = "Usuário Teste";
            var grupoId = 20L;
            var perfil = Guid.NewGuid();
            var grupo = "Grupo Teste";
            var dreId = 30L;
            long? ueId = 40L;
            var turmaId = 50L;
            var inicio = new DateTime(2025, 1, 1, 8, 0, 0);
            var fim = new DateTime(2025, 1, 1, 17, 0, 0);

            var dto = new AbrangenciaDetalheDto
            {
                Id = id,
                UsuarioId = usuarioId,
                Login = login,
                Usuario = usuario,
                GrupoId = grupoId,
                Perfil = perfil,
                Grupo = grupo,
                DreId = dreId,
                UeId = ueId,
                TurmaId = turmaId,
                Inicio = inicio,
                Fim = fim
            };

            Assert.Equal(id, dto.Id);
            Assert.Equal(usuarioId, dto.UsuarioId);
            Assert.Equal(login, dto.Login);
            Assert.Equal(usuario, dto.Usuario);
            Assert.Equal(grupoId, dto.GrupoId);
            Assert.Equal(perfil, dto.Perfil);
            Assert.Equal(grupo, dto.Grupo);
            Assert.Equal(dreId, dto.DreId);
            Assert.Equal(ueId, dto.UeId);
            Assert.Equal(turmaId, dto.TurmaId);
            Assert.Equal(inicio, dto.Inicio);
            Assert.Equal(fim, dto.Fim);
        }

        [Fact]
        public void Deve_Inicializar_Com_Valores_Padrao_Quando_Construtor_Com_Login_Perfil_Eh_Chamado()
        {
            var login = "login.teste";
            var perfil = Guid.NewGuid();

            var dto = new AbrangenciaDetalheDto(login, perfil);

            Assert.Equal(login, dto.Login);
            Assert.Equal(perfil, dto.Perfil);
            Assert.Equal(0, dto.DreId);
            Assert.Equal(0, dto.UeId);
            Assert.Equal(0, dto.TurmaId);
            Assert.Equal(default(DateTime), dto.Inicio);
            Assert.Equal(default(DateTime), dto.Fim);
        }
    }
}
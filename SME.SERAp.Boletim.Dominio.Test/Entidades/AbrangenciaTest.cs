using System;
using SME.SERAp.Boletim.Dominio.Entidades;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Test.Entidades
{
    public class AbrangenciaTest
    {
        [Fact]
        public void Deve_Criar_Abrangencia_Com_Sucesso()
        {
            var usuarioId = 1;
            var grupoId = 2;
            var dreId = 3;
            var ueId = 4;
            var turmaId = 5;
            var inicio = new DateTime(2025, 1, 1);
            var fim = new DateTime(2025, 12, 31);

            var abrangencia = new Abrangencia
            {
                UsuarioId = usuarioId,
                GrupoId = grupoId,
                DreId = dreId,
                UeId = ueId,
                TurmaId = turmaId,
                Inicio = inicio,
                Fim = fim
            };

            Assert.Equal(usuarioId, abrangencia.UsuarioId);
            Assert.Equal(grupoId, abrangencia.GrupoId);
            Assert.Equal(dreId, abrangencia.DreId);
            Assert.Equal(ueId, abrangencia.UeId);
            Assert.Equal(turmaId, abrangencia.TurmaId);
            Assert.Equal(inicio, abrangencia.Inicio);
            Assert.Equal(fim, abrangencia.Fim);
        }
    }
}

using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Infra.Teste.Dtos.BoletimEscolar
{
    public class CardsProficienciaComparativoSmeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Total_Corretamente()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Total = 100
            };

            Assert.Equal(100, dto.Total);
        }

        [Fact]
        public void Deve_Atribuir_Pagina_Corretamente()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Pagina = 2
            };

            Assert.Equal(2, dto.Pagina);
        }

        [Fact]
        public void Deve_Atribuir_ItensPorPagina_Corretamente()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                ItensPorPagina = 10
            };

            Assert.Equal(10, dto.ItensPorPagina);
        }

        [Fact]
        public void Deve_Atribuir_Dres_Corretamente()
        {
            var dres = new List<CardComparativoProficienciaDreDto>
        {
            new CardComparativoProficienciaDreDto(),
            new CardComparativoProficienciaDreDto(),
            new CardComparativoProficienciaDreDto()
        };

            var dto = new CardsProficienciaComparativoSmeDto
            {
                Dres = dres
            };

            Assert.Equal(dres, dto.Dres);
            Assert.Equal(3, dto.Dres.Count());
        }

        [Fact]
        public void Deve_Atribuir_Todas_Propriedades_Corretamente()
        {
            var dres = new List<CardComparativoProficienciaDreDto>
        {
            new CardComparativoProficienciaDreDto(),
            new CardComparativoProficienciaDreDto()
        };

            var dto = new CardsProficienciaComparativoSmeDto
            {
                Total = 50,
                Pagina = 3,
                ItensPorPagina = 15,
                Dres = dres
            };

            Assert.Equal(50, dto.Total);
            Assert.Equal(3, dto.Pagina);
            Assert.Equal(15, dto.ItensPorPagina);
            Assert.Equal(dres, dto.Dres);
            Assert.Equal(2, dto.Dres.Count());
        }

        [Fact]
        public void Deve_Permitir_Total_Zero()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Total = 0
            };

            Assert.Equal(0, dto.Total);
        }

        [Fact]
        public void Deve_Permitir_Pagina_Zero()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Pagina = 0
            };

            Assert.Equal(0, dto.Pagina);
        }

        [Fact]
        public void Deve_Permitir_ItensPorPagina_Zero()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                ItensPorPagina = 0
            };

            Assert.Equal(0, dto.ItensPorPagina);
        }

        [Fact]
        public void Deve_Permitir_Dres_Nulo()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Dres = null
            };

            Assert.Null(dto.Dres);
        }

        [Fact]
        public void Deve_Permitir_Dres_Vazio()
        {
            var dres = new List<CardComparativoProficienciaDreDto>();

            var dto = new CardsProficienciaComparativoSmeDto
            {
                Dres = dres
            };

            Assert.NotNull(dto.Dres);
            Assert.Empty(dto.Dres);
        }

        [Fact]
        public void Deve_Permitir_Dres_Com_Um_Item()
        {
            var dres = new List<CardComparativoProficienciaDreDto>
        {
            new CardComparativoProficienciaDreDto()
        };

            var dto = new CardsProficienciaComparativoSmeDto
            {
                Dres = dres
            };

            Assert.Single(dto.Dres);
        }

        [Fact]
        public void Deve_Permitir_Total_Negativo()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Total = -1
            };

            Assert.Equal(-1, dto.Total);
        }

        [Fact]
        public void Deve_Permitir_Pagina_Negativa()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                Pagina = -1
            };

            Assert.Equal(-1, dto.Pagina);
        }

        [Fact]
        public void Deve_Permitir_ItensPorPagina_Negativo()
        {
            var dto = new CardsProficienciaComparativoSmeDto
            {
                ItensPorPagina = -1
            };

            Assert.Equal(-1, dto.ItensPorPagina);
        }
    }
}

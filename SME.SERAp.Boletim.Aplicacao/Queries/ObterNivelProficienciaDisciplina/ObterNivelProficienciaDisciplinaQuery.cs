using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina
{
    public class ObterNivelProficienciaDisciplinaQuery : IRequest<string>
    {
        public ObterNivelProficienciaDisciplinaQuery(decimal media, long disciplinaId, IEnumerable<ObterNivelProficienciaDto> niveis)
        {
            Media = media;
            DisciplinaId = disciplinaId;
            Niveis = niveis;
        }

        public decimal Media { get; set; }
        public long DisciplinaId { get; set; }
        public IEnumerable<ObterNivelProficienciaDto> Niveis { get; set; }
    }
}
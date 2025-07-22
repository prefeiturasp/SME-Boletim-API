using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolarPorDre;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadBoletimProvaEscolarPorDreUseCase : IObterDownloadBoletimProvaEscolarPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterDownloadBoletimProvaEscolarPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<MemoryStream> Executar(long loteId, long dreId)
        {
            var provas = await mediator.Send(new ObterDownloadProvasBoletimEscolarPorDreQuery(dreId, loteId));
            return await BuildCSVForExcel(provas);
        }

        private static async Task<MemoryStream> BuildCSVForExcel(IEnumerable<DownloadProvasBoletimEscolarPorDreDto> dados)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                await writer.WriteLineAsync("<html><head>");
                await writer.WriteLineAsync("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                await writer.WriteLineAsync("<style>");
                await writer.WriteLineAsync("th { font-weight: bold; }");
                await writer.WriteLineAsync(".numero { text-align: center; }");
                await writer.WriteLineAsync("</style>");
                await writer.WriteLineAsync("</head><body>");
                await writer.WriteLineAsync("<table border='1'>");
                await writer.WriteLineAsync("<tr><th>Nome DRE</th><th>Codigo UE</th><th>Nome UE</th><th>Ano Escola</th><th>Turma</th><th>Aluno RA</th><th>Nome Aluno</th><th>Componente</th><th>Proficiência</th><th>Nível</th></tr>");

                foreach (var item in dados)
                {
                    await writer.WriteLineAsync("<tr>" +
                        $"<td>{item.NomeDreAbreviacao}</td>" +
                        $"<td class=\"numero\">{item.CodigoUE}</td>" +
                        $"<td>{item.NomeUE}</td>" +
                        $"<td class=\"numero\">{item.AnoEscola}</td>" +
                        $"<td>{item.Turma}</td>" +
                        $"<td class=\"numero\">{item.AlunoRA}</td>" +
                        $"<td>{item.NomeAluno}</td>" +
                        $"<td>{item.Componente}</td>" +
                        $"<td class=\"numero\">{item.Proficiencia}</td>" +
                        $"<td>{item.Nivel}</td>" +
                        "</tr>");
                }

                await writer.WriteLineAsync("</table></body></html>");
                await writer.FlushAsync();
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
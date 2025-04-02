using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadResultadoProbabilidade;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadResultadoProbabilidadeUseCase : IObterDownloadResultadoProbabilidadeUseCase
    {
        private readonly IMediator _mediator;

        public ObterDownloadResultadoProbabilidadeUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MemoryStream> Executar(long ueId, long disciplinaId, int anoEscolar)
        {
            var abrangenciasUsuarioLogado = await _mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == Convert.ToInt64(ueId)) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var resultados = await _mediator.Send(new ObterDownloadResultadoProbabilidadeQuery(ueId, disciplinaId, anoEscolar));
            return await BuildCSVForExcel(resultados);
        }

        private static async Task<MemoryStream> BuildCSVForExcel(IEnumerable<DownloadResultadoProbabilidadeDto> resultados)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                await writer.WriteLineAsync("<html><head>");
                await writer.WriteLineAsync("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                await writer.WriteLineAsync("<style>th { font-weight: bold; }</style>");
                await writer.WriteLineAsync("</head><body>");
                await writer.WriteLineAsync("<table border='1'>");
                await writer.WriteLineAsync("<tr><th>Código Habilidade</th><th>Habilidade</th><th>Turma</th><th>Abaixo do Básico</th><th>Básico</th><th>Adequado</th><th>Avançado</th></tr>");

                foreach (var item in resultados)
                {
                    await writer.WriteLineAsync("<tr>" +
                        $"<td>{item.CodigoHabilidade}</td>" +
                        $"<td>{item.HabilidadeDescricao}</td>" +
                        $"<td>{item.TurmaDescricao}</td>" +
                        $"<td>{item.AbaixoDoBasico}</td>" +
                        $"<td>{item.Basico}</td>" +
                        $"<td>{item.Adequado}</td>" +
                        $"<td>{item.Avancado}</td>" +
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
using ClosedXML.Excel;
using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Globalization;
using System.Text;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadResultadoComparativoUseCase : IObterDownloadResultadoComparativoUseCase
    {
        private readonly IMediator mediator;

        // Cores do layout
        private static readonly XLColor CorAzulTitulo = XLColor.FromHtml("#1E6FC9");
        private static readonly XLColor CorAzulInfoUe = XLColor.FromHtml("#5A94D8");
        private static readonly XLColor CorCinzaCabecalho = XLColor.FromHtml("#F2F2F2");
        private static readonly XLColor CorBordaPreta = XLColor.FromHtml("#000000");
        private static readonly XLColor CorBordaCinza = XLColor.FromHtml("#D1D5DB");
        private static readonly XLColor CorTextoEscuro = XLColor.FromHtml("#111827");
        private static readonly XLColor CorBranco = XLColor.White;

        // Cores de nível de proficiência
        private static readonly XLColor CorAbaixoBasicoBg = XLColor.FromHtml("#FEF2F2");
        private static readonly XLColor CorAbaixoBasicoFonte = XLColor.FromHtml("#EF4444");
        private static readonly XLColor CorBasicoBg = XLColor.FromHtml("#EEF2FF");
        private static readonly XLColor CorBasicoFonte = XLColor.FromHtml("#6366F1");
        private static readonly XLColor CorAdequadoBg = XLColor.FromHtml("#ECFDF5");
        private static readonly XLColor CorAdequadoFonte = XLColor.FromHtml("#065F46");
        private static readonly XLColor CorAvancadoBg = XLColor.FromHtml("#FFFBEB");
        private static readonly XLColor CorAvancadoFonte = XLColor.FromHtml("#B45309");

        // Cores de variação
        private static readonly XLColor CorVariacaoPositiva = XLColor.FromHtml("#00B050");
        private static readonly XLColor CorVariacaoNeutra = XLColor.FromHtml("#A5A5A5");
        private static readonly XLColor CorVariacaoNegativa = XLColor.FromHtml("#FF5959");

        private const string LogoUrl = "https://serap.sme.prefeitura.sp.gov.br/files/logo/logo-prefeitura.png";
        private const double LarguraColuna = 35;
        private const double AlturaLinha = 24.75;
        private const double AlturaLogo = 40;
        private const int AlturaLogoPx = (int)(AlturaLogo * 96.0 / 72.0);

        public ObterDownloadResultadoComparativoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<MemoryStream> Executar(int ueId, int disciplinaId, int anoEscolar, long loteId, string? turma, List<int>? tiposVariacao, string? nomeAluno)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var ueDescricao = abrangenciasUsuarioLogado?.FirstOrDefault(x => x.UeId == ueId)?.Descricao ?? string.Empty;

            var dadosProvaSP = await mediator.Send(new ObterNiveisProficienciaComparativoProvaSPQuery(loteId, ueId, disciplinaId, anoEscolar));

            ProficienciaComparativoAlunoSpDto dados;

            if (string.IsNullOrEmpty(turma))
            {
                dados = await mediator
                    .Send(new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(ueId, disciplinaId, anoEscolar, loteId, tiposVariacao, nomeAluno));
            }
            else
            {
                dados = await mediator
                    .Send(new ObterProficienciaComparativoAlunoSpQuery(ueId, disciplinaId, anoEscolar, turma, loteId, tiposVariacao, nomeAluno, null, null));
            }

            if (dados != null)
                dados.UeDescricao = ueDescricao;

            return await BuildXlsx(dadosProvaSP, dados, turma, anoEscolar);
        }

        private static async Task<MemoryStream> BuildXlsx(ProficienciaUeComparacaoProvaSPDto dadosProvaSP, ProficienciaComparativoAlunoSpDto dados, string? turma, int anoEscolar)
        {
            var itens = dados?.Itens?.ToList() ?? new List<ProficienciaAlunoDto>();
            var nomeDisciplina = dados?.NomeDisciplina ?? string.Empty;
            var nomeLote = dados?.NomeLote ?? string.Empty;
            var ueDescricao = dados?.UeDescricao ?? string.Empty;
            var aplicacoes = dados?.Aplicacoes?.ToList() ?? new List<string>();
            var todasColunas = MontarColunas(dadosProvaSP, aplicacoes);
            var totalColunas = todasColunas.Count;
            var totalColunasTabela = totalColunas + 2;

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Comparativo");

            for (int c = 1; c <= totalColunasTabela; c++)
                ws.Column(c).Width = LarguraColuna;

            int row = 1;

            ws.Row(row).Height = AlturaLogo;
            ws.Range(row, 1, row, totalColunasTabela).Merge();
            var logoBytes = await TentarBaixarLogoAsync();
            if (logoBytes != null)
            {
                using var imgStream = new MemoryStream(logoBytes);
                var picture = ws.AddPicture(imgStream)
                    .MoveTo(ws.Cell(row, 1))
                    .WithSize(180, AlturaLogoPx);
            }
            row++;

            row = EscreverCabecalhoRelatorio(ws, row, nomeLote, nomeDisciplina, anoEscolar, totalColunas);

            if (todasColunas.Any())
                row = EscreverSumario(ws, row, todasColunas, dadosProvaSP, itens, totalColunas);

            row = EscreverInfoUe(ws, row, ueDescricao, totalColunas, totalColunasTabela);

            row = EscreverGruposTurma(ws, row, itens, turma, todasColunas, nomeDisciplina, totalColunasTabela);

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        private static List<(string label, bool isPsp)> MontarColunas(ProficienciaUeComparacaoProvaSPDto dadosProvaSP, List<string> aplicacoes)
        {
            var colunas = new List<(string label, bool isPsp)>();

            if (!string.IsNullOrEmpty(dadosProvaSP?.ProvaSP?.NomeAplicacao))
                colunas.Add((dadosProvaSP.ProvaSP.NomeAplicacao + " " + dadosProvaSP.ProvaSP.Periodo, true));

            foreach (var ap in aplicacoes)
                colunas.Add((ap, false));

            return colunas;
        }

        private static int EscreverCabecalhoRelatorio(IXLWorksheet ws, int row, string nomeLote, string nomeDisciplina, int anoEscolar, int totalColunas)
        {
            ws.Row(row).Height = AlturaLinha;
            var cellTitulo = ws.Cell(row, 1);
            cellTitulo.Value = "Boletim de provas - Comparativo";
            EstilarTituloRelatorio(ws.Range(row, 1, row, totalColunas));
            row++;

            ws.Row(row).Height = AlturaLinha;
            var subtitulo = $"{nomeLote} - {nomeDisciplina} - {anoEscolar}º ano";
            ws.Cell(row, 1).Value = subtitulo;
            EstilarSubtituloRelatorio(ws.Range(row, 1, row, totalColunas));
            row++;

            ws.Row(row).Height = AlturaLinha;
            row++;

            return row;
        }

        private static int EscreverSumario(IXLWorksheet ws, int row, List<(string label, bool isPsp)> todasColunas, ProficienciaUeComparacaoProvaSPDto dadosProvaSP, List<ProficienciaAlunoDto> itens, int totalColunas)
        {
            ws.Row(row).Height = AlturaLinha;
            for (int i = 0; i < todasColunas.Count; i++)
                EstilarCabecalho(ws.Cell(row, i + 1)).Value = todasColunas[i].label;
            row++;

            ws.Row(row).Height = AlturaLinha;
            for (int i = 0; i < todasColunas.Count; i++)
            {
                var (label, isPsp) = todasColunas[i];
                var avg = CalcularMediaColuna(dadosProvaSP, itens, label, isPsp);
                var cell = ws.Cell(row, i + 1);
                cell.Value = $"{avg.ToString("N2", new CultureInfo("pt-BR"))} Proficiência";
                var nivel = ObterNivelMaisComum(itens, isPsp ? null : label, isPsp);
                AplicarEstiloNivel(cell, nivel);
            }
            row++;

            ws.Row(row).Height = AlturaLinha;
            for (int i = 0; i < todasColunas.Count; i++)
                EstilarCabecalho(ws.Cell(row, i + 1)).Value = "Estudantes que realizaram a prova";
            row++;

            ws.Row(row).Height = AlturaLinha;
            var total = itens.Count;
            for (int i = 0; i < todasColunas.Count; i++)
            {
                var (label, isPsp) = todasColunas[i];
                var count = CalcularContagemColuna(itens, label, isPsp);
                var pct = total > 0 ? (count * 100.0 / total) : 0;
                var cell = ws.Cell(row, i + 1);
                cell.Value = $"{count} ({pct.ToString("N1", new CultureInfo("pt-BR"))}%)";
                EstilarPadrao(cell);
            }
            row++;

            return row;
        }

        private static int EscreverInfoUe(IXLWorksheet ws, int row, string ueDescricao, int totalColunas, int totalColunasTabela)
        {
            ws.Row(row).Height = AlturaLinha;
            var rangeInfoUe = ws.Range(row, 1, row, totalColunas);
            rangeInfoUe.Merge();
            var cell = ws.Cell(row, 1);
            cell.Value = $"Informações da {ueDescricao} nas provas São Paulo (PSP) e Saberes e Aprendizagens (PSA)";
            cell.Style.Fill.BackgroundColor = CorAzulInfoUe;
            cell.Style.Font.FontColor = CorBranco;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rangeInfoUe.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangeInfoUe.Style.Border.OutsideBorderColor = CorBordaPreta;
            row++;

            ws.Row(row).Height = AlturaLinha;
            row++;

            return row;
        }

        private static int EscreverGruposTurma(IXLWorksheet ws, int row, List<ProficienciaAlunoDto> itens, string? turma, List<(string label, bool isPsp)> todasColunas, string nomeDisciplina, int totalColunasTabela)
        {
            var grupos = string.IsNullOrEmpty(turma)
                ? itens.GroupBy(x => x.Turma ?? string.Empty).OrderBy(g => g.Key)
                : itens.GroupBy(x => turma);

            foreach (var grupo in grupos)
            {
                row = EscreverCabecalhoTurma(ws, row, grupo.Key, nomeDisciplina, todasColunas, totalColunasTabela);
                row = EscreverLinhasAlunos(ws, row, grupo.ToList(), todasColunas);

                ws.Row(row).Height = AlturaLinha;
                row++;
            }

            return row;
        }

        private static int EscreverCabecalhoTurma(IXLWorksheet ws, int row, string nomeTurma, string nomeDisciplina, List<(string label, bool isPsp)> todasColunas, int totalColunasTabela)
        {
            ws.Row(row).Height = AlturaLinha;
            var rangeTitulo = ws.Range(row, 1, row, totalColunasTabela);
            rangeTitulo.Merge();
            var cellTitulo = ws.Cell(row, 1);
            cellTitulo.Value = $"Estudante da turma {nomeTurma} em {nomeDisciplina}";
            cellTitulo.Style.Fill.BackgroundColor = CorAzulTitulo;
            cellTitulo.Style.Font.FontColor = CorBranco;
            cellTitulo.Style.Font.Bold = true;
            cellTitulo.Style.Font.FontName = "Arial";
            cellTitulo.Style.Font.FontSize = 10;
            cellTitulo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cellTitulo.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            row++;

            int rowCabNome = row;
            ws.Row(row).Height = AlturaLinha;

            var cellNome = ws.Cell(row, 1);
            cellNome.Value = "Nome do estudante";
            EstilarCabecalho(cellNome);

            var rangePsa = ws.Range(row, 2, row, todasColunas.Count + 1);
            rangePsa.Merge();
            var cellPsa = ws.Cell(row, 2);
            cellPsa.Value = "Aplicação PSA";
            EstilarCabecalho(cellPsa);

            var cellVariacao = ws.Cell(row, todasColunas.Count + 2);
            cellVariacao.Value = "Variação";
            EstilarCabecalho(cellVariacao);
            row++;

            ws.Row(row).Height = AlturaLinha;
            var rangeNome = ws.Range(rowCabNome, 1, row, 1);
            rangeNome.Merge();
            var rangeVar = ws.Range(rowCabNome, todasColunas.Count + 2, row, todasColunas.Count + 2);
            rangeVar.Merge();
            AplicarBordaPreta(ws.Cell(rowCabNome, 1).Style.Border);
            AplicarBordaPreta(ws.Cell(rowCabNome, todasColunas.Count + 2).Style.Border);

            for (int i = 0; i < todasColunas.Count; i++)
            {
                var (label, isPsp) = todasColunas[i];
                var subLabel = isPsp ? $"PSP ({label.ToLower()})" : $"PSA ({label.ToLower()})";
                EstilarSubcabecalho(ws.Cell(row, i + 2)).Value = subLabel;
            }

            rangePsa.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangePsa.Style.Border.OutsideBorderColor = CorBordaPreta;
            rangeNome.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangeNome.Style.Border.OutsideBorderColor = CorBordaPreta;
            AplicarBordaPreta(ws.Cell(rowCabNome, todasColunas.Count + 2).Style.Border);
            AplicarBordaPreta(ws.Cell(row, todasColunas.Count + 2).Style.Border);
            row++;

            return row;
        }

        private static int EscreverLinhasAlunos(IXLWorksheet ws, int row, List<ProficienciaAlunoDto> alunos, List<(string label, bool isPsp)> todasColunas)
        {
            for (int idx = 0; idx < alunos.Count; idx++)
            {
                var aluno = alunos[idx];
                ws.Row(row).Height = AlturaLinha;
                var alt = idx % 2 != 0;

                var cellNome = ws.Cell(row, 1);
                cellNome.Value = aluno.Nome;
                if (alt) EstilarLinhaAlternada(cellNome); else EstilarPadrao(cellNome);

                for (int i = 0; i < todasColunas.Count; i++)
                {
                    var (label, isPsp) = todasColunas[i];
                    var prof = isPsp
                        ? aluno.Proficiencias?.FirstOrDefault(p => string.IsNullOrEmpty(p.Mes))
                        : aluno.Proficiencias?.FirstOrDefault(p => p.Mes == label);

                    var cell = ws.Cell(row, i + 2);
                    if (prof != null)
                    {
                        cell.Value = prof.Valor.ToString("N2", new CultureInfo("pt-BR"));
                        AplicarEstiloNivel(cell, prof.NivelProficiencia);
                    }
                    else
                    {
                        cell.Value = "-";
                        if (alt) EstilarLinhaAlternada(cell); else EstilarPadrao(cell);
                    }
                }

                var cellVar = ws.Cell(row, todasColunas.Count + 2);
                cellVar.Value = FormatarVariacao(aluno.Variacao);
                AplicarEstiloVariacao(cellVar, aluno.Variacao);

                row++;
            }

            return row;
        }

        private static IXLCell EstilarTituloRelatorio(IXLRange range)
        {
            range.Merge();
            var cell = range.FirstCell();
            cell.Style.Fill.BackgroundColor = CorAzulTitulo;
            cell.Style.Font.FontColor = CorBranco;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            return cell;
        }

        private static IXLCell EstilarSubtituloRelatorio(IXLRange range)
        {
            range.Merge();
            var cell = range.FirstCell();
            cell.Style.Fill.BackgroundColor = CorAzulTitulo;
            cell.Style.Font.FontColor = CorBranco;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            return cell;
        }

        private static IXLCell EstilarCabecalho(IXLCell cell)
        {
            cell.Style.Fill.BackgroundColor = CorCinzaCabecalho;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            AplicarBordaPreta(cell.Style.Border);
            return cell;
        }

        private static IXLCell EstilarSubcabecalho(IXLCell cell)
        {
            cell.Style.Fill.BackgroundColor = CorCinzaCabecalho;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorderColor = CorBordaPreta;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorderColor = CorBordaPreta;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorderColor = CorBordaPreta;
            return cell;
        }

        private static IXLCell EstilarPadrao(IXLCell cell)
        {
            cell.Style.Fill.BackgroundColor = CorBranco;
            cell.Style.Font.FontColor = CorTextoEscuro;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            AplicarBordaCinza(cell.Style.Border);
            return cell;
        }

        private static IXLCell EstilarLinhaAlternada(IXLCell cell)
        {
            EstilarPadrao(cell);
            cell.Style.Fill.BackgroundColor = CorCinzaCabecalho;
            return cell;
        }

        private static void AplicarEstiloNivel(IXLCell cell, string? nivel)
        {
            var nivelNorm = nivel is null ? null : RemoverAcentos(nivel.ToLower());
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            AplicarBordaCinza(cell.Style.Border);

            switch (nivelNorm)
            {
                case "abaixo do basico":
                    cell.Style.Fill.BackgroundColor = CorAbaixoBasicoBg;
                    cell.Style.Font.FontColor = CorAbaixoBasicoFonte;
                    break;
                case "basico":
                    cell.Style.Fill.BackgroundColor = CorBasicoBg;
                    cell.Style.Font.FontColor = CorBasicoFonte;
                    break;
                case "adequado":
                    cell.Style.Fill.BackgroundColor = CorAdequadoBg;
                    cell.Style.Font.FontColor = CorAdequadoFonte;
                    break;
                case "avancado":
                    cell.Style.Fill.BackgroundColor = CorAvancadoBg;
                    cell.Style.Font.FontColor = CorAvancadoFonte;
                    break;
                default:
                    cell.Style.Fill.BackgroundColor = CorBranco;
                    cell.Style.Font.FontColor = CorTextoEscuro;
                    break;
            }
        }

        private static void AplicarEstiloVariacao(IXLCell cell, double variacao)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = CorBranco;
            cell.Style.Font.FontName = "Arial";
            cell.Style.Font.FontSize = 10;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            AplicarBordaCinza(cell.Style.Border);
            cell.Style.Fill.BackgroundColor = variacao > 0 ? CorVariacaoPositiva : variacao < 0 ? CorVariacaoNegativa : CorVariacaoNeutra;
        }

        private static void AplicarBordaPreta(IXLBorder border)
        {
            border.TopBorder = border.BottomBorder = border.LeftBorder = border.RightBorder = XLBorderStyleValues.Thin;
            border.TopBorderColor = border.BottomBorderColor = border.LeftBorderColor = border.RightBorderColor = CorBordaPreta;
        }

        private static void AplicarBordaCinza(IXLBorder border)
        {
            border.TopBorder = border.BottomBorder = border.LeftBorder = border.RightBorder = XLBorderStyleValues.Thin;
            border.TopBorderColor = border.BottomBorderColor = border.LeftBorderColor = border.RightBorderColor = CorBordaCinza;
        }

        private static decimal CalcularMediaColuna(ProficienciaUeComparacaoProvaSPDto dadosProvaSP, List<ProficienciaAlunoDto> itens, string label, bool isPsp)
        {
            if (isPsp)
                return dadosProvaSP?.ProvaSP?.MediaProficiencia ?? 0;

            return itens
                .SelectMany(i => i.Proficiencias ?? Enumerable.Empty<ProficienciaDetalheDto>())
                .Where(p => p.Mes == label)
                .Select(p => p.Valor)
                .DefaultIfEmpty(0)
                .Average();
        }

        private static int CalcularContagemColuna(List<ProficienciaAlunoDto> itens, string label, bool isPsp)
        {
            return isPsp
                ? itens.Count(x => x.Proficiencias != null && x.Proficiencias.Any(p => string.IsNullOrEmpty(p.Mes)))
                : itens.Count(x => x.Proficiencias != null && x.Proficiencias.Any(p => p.Mes == label));
        }

        private static string? ObterNivelMaisComum(List<ProficienciaAlunoDto> itens, string? periodo, bool isPsp)
        {
            var niveis = isPsp
                ? itens.SelectMany(i => i.Proficiencias ?? Enumerable.Empty<ProficienciaDetalheDto>())
                       .Where(p => string.IsNullOrEmpty(p.Mes))
                       .Select(p => p.NivelProficiencia)
                : itens.SelectMany(i => i.Proficiencias ?? Enumerable.Empty<ProficienciaDetalheDto>())
                       .Where(p => p.Mes == periodo)
                       .Select(p => p.NivelProficiencia);

            return niveis.GroupBy(n => n).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;
        }

        private static string RemoverAcentos(string texto)
        {
            var normalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalizado)
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string FormatarVariacao(double variacao)
        {
            if (variacao == 0) return "0,0%";
            return $"{variacao.ToString("N2", new CultureInfo("pt-BR"))}%";
        }

        private static async Task<byte[]?> TentarBaixarLogoAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                return await httpClient.GetByteArrayAsync(LogoUrl);
            }
            catch
            {
                return null;
            }
        }
    }
}

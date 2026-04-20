using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Luxprop.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Luxprop.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly LuxpropContext _db;

        public ReportsController(LuxpropContext db)
        {
            _db = db;
        }

        // GET /Reports/CaseRecordsPdf?... (mismos filtros que Records.razor)
        [HttpGet("CaseRecordsPdf")]
        
        public async Task<IActionResult> CaseRecordsPdf(
    [FromQuery] int? expedienteId,
    [FromQuery] string? estado,
    [FromQuery] string? search,
    [FromQuery] string? from,
    [FromQuery] string? to)
        {
            DateOnly? fromDate = ParseDateOnly(from);
            DateOnly? toDate = ParseDateOnly(to);

            var q = _db.Expedientes
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente).ThenInclude(c => c.Usuario)
                .Include(e => e.Documentos)
                .Include(e => e.TareaTramites)
                .AsQueryable();

            if (expedienteId.HasValue)
                q = q.Where(e => e.ExpedienteId == expedienteId.Value);

            if (!string.IsNullOrWhiteSpace(estado))
                q = q.Where(e => e.Estado == estado);

            if (fromDate.HasValue)
                q = q.Where(e => e.FechaApertura.HasValue && e.FechaApertura.Value >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(e => e.FechaApertura.HasValue && e.FechaApertura.Value <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                q = q.Where(e =>
                    (e.Propiedad != null && e.Propiedad.Titulo != null &&
                     e.Propiedad.Titulo.ToLower().Contains(term)) ||
                    (e.Cliente != null && e.Cliente.Usuario != null &&
                     (e.Cliente.Usuario.Nombre + " " + e.Cliente.Usuario.Apellido)
                        .ToLower().Contains(term)));
            }

            var list = await q
                .OrderByDescending(e => e.FechaApertura)
                .ThenBy(e => e.ExpedienteId)
                .ToListAsync();

            var rows = list.Select(e =>
            {
                var totalTasks = e.TareaTramites?.Count ?? 0;
                var completedTasks = e.TareaTramites?.Count(t =>
                    t.Estado == "Completed" ||
                    t.Estado == "Completado" ||
                    t.Estado == "Finalizado" ||
                    t.Estado == "Cerrado") ?? 0;

                var progress = totalTasks > 0
                    ? (double)completedTasks / totalTasks * 100.0
                    : 0;

                return new CaseRecordRow
                {
                    Id = e.ExpedienteId,
                    Property = e.Propiedad?.Titulo ?? "-",
                    Client = e.Cliente?.Usuario != null
                        ? $"{e.Cliente.Usuario.Nombre} {e.Cliente.Usuario.Apellido}"
                        : "-",
                    Status = e.Estado ?? "-",
                    Opened = e.FechaApertura,
                    Closed = e.FechaCierre,
                    DocumentCount = e.Documentos?.Count ?? 0,
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    ProgressPercent = progress
                };
            }).ToList();

            /* ================= PDF ================= */

            var logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "img", "logo-2crre.png"
            );

            var primaryColor = "#0d505a";
            var lightBg = "#f3f8f8";

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    /* ===== HEADER ===== */
                    page.Header()
                        .Background(primaryColor)
                        .PaddingVertical(16)
                        .PaddingHorizontal(24)
                        .Row(row =>
                        {
                            row.ConstantItem(100)
                                .AlignMiddle()
                                .Height(40)
                                .Image(logoPath);

                            row.RelativeItem()
                                .AlignMiddle()
                                .Column(col =>
                                {
                                    col.Item().Text("CASE RECORDS REPORT")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.White);

                                    col.Item().Text("Administrative Case Overview")
                                        .FontSize(12)
                                        .FontColor("#E0F2F4");
                                });
                        });

                    /* ===== CONTENT ===== */
                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Item().Text("Case Records")
                            .FontSize(14)
                            .Bold()
                            .FontColor(primaryColor);

                        column.Item().LineHorizontal(1).LineColor(primaryColor);
                        column.Item().PaddingTop(10);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(60);   // ID
                                columns.RelativeColumn(2);    // Property
                                columns.RelativeColumn(2);    // Client
                                columns.ConstantColumn(90);   // Status
                                columns.ConstantColumn(90);   // Opened
                                columns.ConstantColumn(90);   // Closed
                                columns.ConstantColumn(60);   // Docs
                                columns.ConstantColumn(80);   // Tasks
                                columns.ConstantColumn(80);   // Progress
                            });

                            /* HEADER */
                            table.Header(header =>
                            {
                                header.Cell().Element(c => HeaderStyle(c, "ID"));
                                header.Cell().Element(c => HeaderStyle(c, "Property"));
                                header.Cell().Element(c => HeaderStyle(c, "Client"));
                                header.Cell().Element(c => HeaderStyle(c, "Status"));
                                header.Cell().Element(c => HeaderStyle(c, "Opened"));
                                header.Cell().Element(c => HeaderStyle(c, "Closed"));
                                header.Cell().Element(c => HeaderStyle(c, "Docs"));
                                header.Cell().Element(c => HeaderStyle(c, "Tasks"));
                                header.Cell().Element(c => HeaderStyle(c, "Progress"));
                            });


                            /* ROWS */
                            bool alternate = false;

                            foreach (var r in rows)
                            {
                                var bg = alternate ? lightBg : "#FFFFFF";
                                alternate = !alternate;

                                DataCell(table, bg, r.Id.ToString());
                                DataCell(table, bg, r.Property);
                                DataCell(table, bg, r.Client);
                                DataCell(table, bg, r.Status);
                                DataCell(table, bg, r.Opened?.ToString("yyyy-MM-dd"));
                                DataCell(table, bg, r.Closed?.ToString("yyyy-MM-dd"));
                                DataCell(table, bg, r.DocumentCount.ToString());
                                DataCell(table, bg, $"{r.CompletedTasks}/{r.TotalTasks}");
                                DataCell(table, bg, $"{r.ProgressPercent:0}%");
                            }
                        });
                    });

                    /* ===== FOOTER ===== */
                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("2CRRE Docs • ")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Medium);

                            txt.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                                .FontSize(9)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", "case-records.pdf");
        }

        static void HeaderStyle(IContainer c, string text)
        {
            c.Padding(6)
             .Background("#0d505a")
             .AlignCenter()
             .Text(text)
             .Bold()
             .FontColor(Colors.White);
        }


        static void DataCell(TableDescriptor table, string bg, string? text)
        {
            table.Cell().Element(c => c
                .Padding(6)
                .Background(bg)
                .BorderBottom(1)
                .BorderColor("#cfdede")
                .Text(text ?? "-")
            );
        }

        private static DateOnly? ParseDateOnly(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date))
            {
                return date;
            }

            return null;
        }
    }

    // DTO simple para el PDF
    public class CaseRecordRow
    {
        public int Id { get; set; }
        public string Property { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly? Opened { get; set; }
        public DateOnly? Closed { get; set; }
        public int DocumentCount { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double ProgressPercent { get; set; }
    }

    // Documento PDF con QuestPDF
    
        /* ================= HELPERS ================= */
       


    }



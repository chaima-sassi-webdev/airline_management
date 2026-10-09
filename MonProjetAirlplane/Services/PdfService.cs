using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MonProjetAirlplane.Models;

namespace MonProjetAirlplane.Services
{
    public class PdfService
    {
        public PdfService()
        {
            // Configuration de la licence communautaire gratuite
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;
        }

        public byte[] GenerateFlightReport(IEnumerable<MarketDetails> flights)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Lato"));

                    // En-tête (affiché sur chaque page ou seulement la première avec ShowOnce si souhaité)
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("TUNISAIR").Bold().FontSize(20).FontColor(Colors.Blue.Medium);
                            col.Item().Text("Rapport Officiel des Vols et Marchés").FontSize(12).FontColor(Colors.Grey.Medium);
                        });
                        row.ConstantItem(100).AlignRight().Text($"Date : {DateTime.Now:dd/MM/yyyy}").FontSize(9);
                    });

                    // Contenu principal : le tableau s'étalera automatiquement sur plusieurs pages si nécessaire
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        // Définition des colonnes
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70); // Vol
                            columns.ConstantColumn(80); // Immat
                            columns.ConstantColumn(80); // Date
                            columns.ConstantColumn(70); // Escale
                            columns.RelativeColumn();   // Tarif Moyen
                        });

                        // En-têtes du tableau (répétés automatiquement sur chaque nouvelle page)
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Vol").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Immat").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Date").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Escale").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Tarif Moyen").FontColor(Colors.White).Bold();
                        });

                        // Remplissage de toutes les lignes de la liste
                        foreach (var flight in flights)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{flight.FN_CARRIER} {flight.FN_NUMBER}");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(flight.AC_REGISTRATION ?? "-");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(flight.DAY_OF_ORIGIN);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(flight.ARR_AP_ACTUAL ?? "-");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{flight.AVG_FARE:N2} TND");
                        }
                    });

                    // Pied de page avec numérotation dynamique des pages
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" sur ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
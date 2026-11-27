using ClosedXML.Excel;

namespace Kabellijster;

public static class ExcelReader
{
    public static List<Cable> Read(string path, ExcelSettings settings)
    {
        settings.Validate();
        using var workbook = new XLWorkbook(path);
        var cables = new List<Cable>();

        foreach (var sheet in workbook.Worksheets)
        {
            var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 0;
            for (var row = settings.EersteDatarij; row <= lastRow; row++)
            {
                if (sheet.Row(row).IsEmpty()) continue;
                string ReadCell(string column) => sheet.Cell(row, column).GetFormattedString().Trim();

                var cable = new Cable(
                    ReadCell(settings.Kabelnaam),
                    ReadCell(settings.Kabeltype),
                    ReadCell(settings.Diameter),
                    ReadCell(settings.Aders),
                    ReadCell(settings.Bronkast),
                    ReadCell(settings.Doelkast),
                    sheet.Name,
                    row);

                cables.Add(cable);
            }
        }

        return cables;
    }
}

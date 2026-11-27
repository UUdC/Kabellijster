using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kabellijster;

public sealed class ExcelSettings
{
    [JsonRequired] public int EersteDatarij { get; init; }
    [JsonRequired] public string Kabelnaam { get; init; } = "";
    [JsonRequired] public string Kabeltype { get; init; } = "";
    [JsonRequired] public string Diameter { get; init; } = "";
    [JsonRequired] public string Aders { get; init; } = "";
    [JsonRequired] public string Bronkast { get; init; } = "";
    [JsonRequired] public string Doelkast { get; init; } = "";

    public static ExcelSettings Load(string path)
    {
        var settings = JsonSerializer.Deserialize<ExcelSettings>(File.ReadAllText(path))
            ?? throw new InvalidDataException("De Excel-instellingen zijn leeg.");
        settings.Validate();
        return settings;
    }

    public void Validate()
    {
        if (EersteDatarij < 1 || EersteDatarij > 1_048_576)
            throw new InvalidDataException("EersteDatarij moet tussen 1 en 1048576 liggen.");

        var columns = new[] { Kabelnaam, Kabeltype, Diameter, Aders, Bronkast, Doelkast };
        foreach (var column in columns)
        {
            if (string.IsNullOrEmpty(column) || column.Length > 3 ||
                column.Any(c => c < 'A' || c > 'Z'))
                throw new InvalidDataException("Gebruik hoofdletters A t/m XFD voor iedere Excel-kolom.");

            var number = 0;
            foreach (var c in column) number = number * 26 + c - 'A' + 1;
            if (number > 16_384)
                throw new InvalidDataException("Gebruik hoofdletters A t/m XFD voor iedere Excel-kolom.");
        }

        if (columns.Distinct(StringComparer.Ordinal).Count() != columns.Length)
            throw new InvalidDataException("Elke eigenschap moet een eigen Excel-kolom hebben.");
    }
}

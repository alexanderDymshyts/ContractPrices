using System.Globalization;

namespace Models;

public class CsvIncomeRow
{
 #region Properties

    public DateOnly Start { get; set; }

    public DateOnly End { get; set; }

    public decimal Price { get; set; }

    public int Priority { get; set; }

    #endregion Properties

    #region Methods

    public static CsvIncomeRow ToCsvIncomeRow(Csv fileData)
    {
        DateOnly.TryParseExact(fileData.Start, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start);
        DateOnly.TryParseExact(fileData.Ende, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var end);
        decimal.TryParse(fileData.Preis.Replace(',', '.'), out var price);
        return new CsvIncomeRow
        {
            Start = start, 
            End = end,
            Price = price,
            Priority = fileData.Priorität
        };
    }

    #endregion Methods
}
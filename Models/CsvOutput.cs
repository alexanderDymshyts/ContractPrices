using System.Globalization;

namespace Models;

public class CsvOutput
{
    #region Properties

    public string Start { get; set; }

    public string Ende { get; set; }

    public string Preis { get; set; }

    #endregion Properties

    #region Methods

    public static CsvOutput ToCsvOutput(CsvReturnRow item)
    {
        return new CsvOutput
        {
            Start = item.Start.ToString("dd.MM.yyyy"),
            Ende = item.End.ToString("dd.MM.yyyy"),
            Preis = item.Price.ToString(CultureInfo.InvariantCulture).Replace('.', ',')
        };
    }

    #endregion Methods
}
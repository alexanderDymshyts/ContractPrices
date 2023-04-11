using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Logic.Classes;

using Interfaces;
using Models;

public class CsvHelper: ICsv
{
    #region Methods

    /// <summary>
    /// Read data from csv.
    /// </summary>
    /// <param name="fullpath">Full file path with file name.</param>
    /// <returns>Collection of csv rows.</returns>
    public List<CsvIncomeRow> ReadCsvFile(string fullpath)
    {
        if (string.IsNullOrEmpty(fullpath))
            return new List<CsvIncomeRow>();
        
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Comment = '#',
            AllowComments = false,
            Delimiter = ";"
        };

        using var streamReader = File.OpenText(fullpath);
        using var csvReader = new CsvReader(streamReader, csvConfig);

        return csvReader.GetRecords<Csv>().Select(CsvIncomeRow.ToCsvIncomeRow).ToList();
    }

    /// <summary>
    /// Write csv to file.
    /// </summary>
    /// <param name="items">Items to write in file.</param>
    /// <returns>Path, where csv was saved.</returns>
    public string WriteCsvFile(List<CsvReturnRow> items)
    {
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),"result.csv" );
        
        using var textWriter = new StreamWriter(fullPath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
        };
        var writer = new CsvWriter(textWriter, config);
        writer.WriteRecords(items.Select(CsvOutput.ToCsvOutput));

        return fullPath;
    }

    #endregion Methods
}
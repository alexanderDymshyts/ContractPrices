namespace Logic.Interfaces;

using Models;

public interface ICsv
{
    /// <summary>
    /// Read data from csv.
    /// </summary>
    /// <param name="fullpath">Full file path with file name.</param>
    /// <returns>Collection of csv rows.</returns>
    List<CsvIncomeRow> ReadCsvFile(string fullpath);

    /// <summary>
    /// Write csv to file.
    /// </summary>
    /// <param name="items">Items to write in file.</param>
    /// <returns>Path, where csv was saved.</returns>
    string WriteCsvFile(List<CsvReturnRow> items);
}
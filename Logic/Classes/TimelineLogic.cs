namespace Logic.Classes;

using Interfaces;
using Models;
using System.Linq;

public class TimelineLogic: ITimeline
{
    #region Methods

    /// <summary>
    /// Get resulting timeline for csv file.
    /// </summary>
    /// <param name="csvRows">Incoming csv rows.</param>
    /// <returns>Merged rows.</returns>
    public List<CsvReturnRow> GetTimeline(List<CsvIncomeRow> csvRows)
    {
        if (csvRows == null || csvRows.Count == 0)
            return new List<CsvReturnRow>();

        var rows = PrepareData(csvRows);
        
        // Query for priority sort.
        IEnumerable<CsvIncomeRow> sortQuery = 
            from row in rows
            orderby row.Priority descending 
            select row;

        List<CsvReturnRow> result = new ();

        foreach (var year in sortQuery.DistinctBy(x => x.Start.Year).Select(x => x.Start.Year))
        {
            int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
            decimal?[] prices = new decimal?[daysInYear];

            var oneYearCollection = sortQuery.Where(x => x.Start.Year == year).ToList();
            FillArrayWithPrices(oneYearCollection, prices);

            ExtractPrices(year, daysInYear, prices, result);
        }

        return result;
    }

    /// <summary>
    /// Fill array with correct prices according to priority.
    /// </summary>
    /// <param name="oneYearCollection">Collection on timelines for 1 year.</param>
    /// <param name="prices">Array where prices should be saved.</param>
    private void FillArrayWithPrices(List<CsvIncomeRow> oneYearCollection, decimal?[] prices)
    {
        // Put all intervals
        foreach (var interval in oneYearCollection)
        {
            int startIndex = interval.Start.DayOfYear - 1; 
            int endIndex = interval.End.DayOfYear - 1;
            for (int i = startIndex; i <= endIndex; i++)
            {
                prices[i] ??= interval.Price;
            }
        }
    }
    
    /// <summary>
    /// Extract prices from array to resulting collection.
    /// </summary>
    /// <param name="year">Year of the collection.</param>
    /// <param name="daysInYear">How many days in year (leap/ not leap year).</param>
    /// <param name="prices">Collection with saved prices.</param>
    /// <param name="result">Resulting collection to save prices.</param>
    private void ExtractPrices(int year, int daysInYear, decimal?[] prices, List<CsvReturnRow> result)
    {
        int lastIndex = daysInYear - 1;
        for (int i = 0; i < daysInYear; i++)
        {
            if (prices[i].HasValue)
            {
                for (int j = i + 1; j < daysInYear; j++)
                {
                    if (j == lastIndex) // Add last item
                    {
                        result.Add(new CsvReturnRow
                        {
                            Start = new DateOnly(year, 1, 1).AddDays(i),
                            End = new DateOnly(year, 1, 1).AddDays(j),
                            Price = prices[i].Value,
                        });
                        i = lastIndex;
                        break;
                    }
                    
                    if (prices[i] != prices[j])
                    {
                        int nextIndex = j - 1;
                        result.Add(new CsvReturnRow
                        {
                            Start = new DateOnly(year, 1, 1).AddDays(i),
                            End = new DateOnly(year, 1, 1).AddDays(nextIndex),
                            Price = prices[i].Value,
                        });

                        i = nextIndex;
                        break;
                    }
                }
            }
        }
    }
 
    /// <summary>
    /// Split multiple years in one row to 2 separate dates.
    /// </summary>
    /// <param name="original">Original list without duplicates.</param>
    /// <returns>Rows in one year only.</returns>
    private List<CsvIncomeRow> PrepareData(List<CsvIncomeRow> original)
    {
        List<CsvIncomeRow> oneYearPerRow = new();
        
        foreach (var item in original)
        {
            if (!item.Start.Year.Equals(item.End.Year))
            {
                oneYearPerRow.Add(new CsvIncomeRow
                {
                    Start = item.Start,
                    Price = item.Price,
                    Priority = item.Priority,
                    End = new DateOnly(item.Start.Year, 12, 31)
                });
                
                oneYearPerRow.Add(new CsvIncomeRow
                {
                    Start = new DateOnly(item.End.Year, 1, 1),
                    Price = item.Price,
                    Priority = item.Priority,
                    End = item.End
                });
            }
            else
            {
                oneYearPerRow.Add(item);
            }
        }

        return oneYearPerRow;
    }

    #endregion Methods
}
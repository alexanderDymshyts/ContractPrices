namespace Logic.Interfaces;

using Models;

public interface ITimeline
{
    List<CsvReturnRow> GetTimeline(List<CsvIncomeRow> csvRows);
}
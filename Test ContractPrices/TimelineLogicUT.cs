using System.Linq;

namespace Test_ContractPrices;

using System;
using System.Collections.Generic;
using Logic.Classes;
using Models;
using Xunit;

public class TimelineLogicUT
{
    #region Variables

    private TimelineLogic _timelineLogic = new TimelineLogic();

    #endregion Variables

    #region Tests

    [Fact]
    public void SameStartDateAscPriority()
    {
        List<CsvIncomeRow> data =  new()
        { 
            new CsvIncomeRow
            {
                Start = new DateOnly(2023,4, 1),
                End = new DateOnly(2023, 6, 30),
                Priority = 7,
                Price = 25
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2023,4, 1),
                End = new DateOnly(2023, 6, 15),
                Priority = 8,
                Price = 10
            },
        };

        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2023, 4, 1), End = new DateOnly(2023, 6, 15), Price = 10 },
            new CsvReturnRow{ Start = new DateOnly(2023, 6, 16), End = new DateOnly(2023, 6, 30), Price = 25 }
        };

        var actual = _timelineLogic.GetTimeline(data);
       
        CollectionsAreEqual(expected, actual);
    }
    
    [Fact]
    public void SameStartDateDescPriority()
    {
        List<CsvIncomeRow> data =  new()
        { 
            new CsvIncomeRow
            {
                Start = new DateOnly(2023,4, 1),
                End = new DateOnly(2023, 6, 30),
                Priority = 8,
                Price = 25
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2023,4, 1),
                End = new DateOnly(2023, 6, 15),
                Priority = 7,
                Price = 10
            },
        };

        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2023, 4, 1), End = new DateOnly(2023, 6, 30), Price = 25 }
        };

        var actual = _timelineLogic.GetTimeline(data);
       
        CollectionsAreEqual(expected, actual);
    }

    [Fact]
    public void YearOverlap()
    {
        List<CsvIncomeRow> data = new()
        {
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,7, 1),
                End = new DateOnly(2021, 6, 30),
                Priority = 11,
                Price = 18
            }
        };
        
        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2020, 7, 1), End = new DateOnly(2020, 12, 31), Price = 18 },
            new CsvReturnRow{ Start = new DateOnly(2021, 1, 1), End = new DateOnly(2021, 6, 30), Price = 18 },
        };

        var actual = _timelineLogic.GetTimeline(data);
        
        CollectionsAreEqual(expected, actual);
    }
    
    [Fact]
    public void EqualDatesHighPriorityFirst()
    {
        List<CsvIncomeRow> data = new()
        {
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,6, 1),
                End = new DateOnly(2020, 7, 30),
                Priority = 11,
                Price = 18
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,6, 1),
                End = new DateOnly(2020, 7, 30),
                Priority = 10,
                Price = 15
            }
        };
        
        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2020, 6, 1), End = new DateOnly(2020, 7, 30), Price = 18 }
        };

        var actual = _timelineLogic.GetTimeline(data);
        
        CollectionsAreEqual(expected, actual);
    }
    
    [Fact]
    public void EqualDatesHighPriorityLast()
    {
        List<CsvIncomeRow> data = new()
        {
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,6, 1),
                End = new DateOnly(2020, 7, 30),
                Priority = 11,
                Price = 18
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,6, 1),
                End = new DateOnly(2020, 7, 30),
                Priority = 12,
                Price = 15
            }
        };
        
        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2020, 6, 1), End = new DateOnly(2020, 7, 30), Price = 15 }
        };

        var actual = _timelineLogic.GetTimeline(data);
        
        CollectionsAreEqual(expected, actual);
    }
    
    [Fact]
    public void TimelinesInsideOneYear()
    {
        List<CsvIncomeRow> data = new()
        {
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,1, 1),
                End = new DateOnly(2020, 3, 1),
                Priority = 1,
                Price = 20.40M
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,2, 1),
                End = new DateOnly(2020, 12, 31),
                Priority = 2,
                Price = 18
            },
            new CsvIncomeRow
            {
                Start = new DateOnly(2020,6, 1),
                End = new DateOnly(2020, 9, 1),
                Priority = 3,
                Price = 16.90M
            }
        };
        
        var expected = new List<CsvReturnRow>
        {
            new CsvReturnRow{ Start = new DateOnly(2020, 1, 1), End = new DateOnly(2020, 1, 31), Price = 20.40M },
            new CsvReturnRow{ Start = new DateOnly(2020, 2, 1), End = new DateOnly(2020, 5, 31), Price = 18 },
            new CsvReturnRow{ Start = new DateOnly(2020, 6, 1), End = new DateOnly(2020, 9, 1), Price = 16.90M },
            new CsvReturnRow{ Start = new DateOnly(2020, 9, 2), End = new DateOnly(2020, 12, 31), Price = 18 },
        };

        var actual = _timelineLogic.GetTimeline(data);
        
        CollectionsAreEqual(expected, actual);
    }

    #endregion Tests

    #region Methods

    private void CollectionsAreEqual(List<CsvReturnRow> expected, List<CsvReturnRow> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Start, actual[i].Start);
            Assert.Equal(expected[i].End, actual[i].End);
            Assert.Equal(expected[i].Price, actual[i].Price);
        }
    }

    #endregion Methods
}
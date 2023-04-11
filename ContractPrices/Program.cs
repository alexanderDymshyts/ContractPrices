// See https://aka.ms/new-console-template for more information

using Logic.Classes;
using Logic.Interfaces;

string path = String.Empty;

while (string.IsNullOrEmpty(path))
{
    Console.WriteLine("Type q for exit.");
    Console.WriteLine("Please enter csv path including file name and extension:");
    path = Console.ReadLine();
}

if (!path.Equals("q"))
{
    ICsv csv = new Logic.Classes.CsvHelper();
    var items = csv.ReadCsvFile(path);
    if (items.Count() != 0)
    {
        ITimeline timelineLogic = new TimelineLogic();
        var result = timelineLogic.GetTimeline(items);
        if (result.Count != 0)
        {
            string outputPath = csv.WriteCsvFile(result);
            Console.WriteLine("File can be found here:" + outputPath);
        }
    }
}

Console.WriteLine("Done...");
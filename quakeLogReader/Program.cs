using System;
using System.IO;
using quakeLogReader.Common;
using quakeLogReader.Report;

namespace quakeLogReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Quake 3 Arena log reader!");
            Console.WriteLine("Let's read the file...");

            Quake3ArenaLogReader logReader = new Quake3ArenaLogReader();
            logReader.ReadLog(@$"{Directory.GetCurrentDirectory()}\input\qgames.log");

            Console.WriteLine("Finished!");
            Console.WriteLine("Showing results...");

            ShowResults(logReader);
        }

        private static void ShowResults(Quake3ArenaLogReader logReader)
        {
            if (logReader.HasErrors)
            {
                ShowErrors(logReader);
            }
            else
            {
                ShowReport(logReader);
            }
        }

        private static void ShowErrors(Quake3ArenaLogReader logReader)
        {
            Console.WriteLine("There are errors... :(");
            foreach (var row in logReader.Errors)
            {
                Console.WriteLine(row);
            }
        }

        private static void ShowReport(Quake3ArenaLogReader logReader)
        {
            Console.WriteLine($"Total games: {logReader.Games.Count}");
            ReportData report = null;
            foreach (var game in logReader.Games)
            {
                report = new ReportData(game);
                Console.WriteLine(report.ToString());
            }
        }
    }
}

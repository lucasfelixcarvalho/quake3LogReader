using System;

namespace quakeLogReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Quake 3 Arena log reader!");
            Console.WriteLine("Let's read the file...");

            Quake3ArenaLogReader logReader = new Quake3ArenaLogReader();
            logReader.ReadLog(@"E:\Documents\Git\quake3LogReader\quakeLogReader\input\logForTest.log");

            Console.WriteLine("Finished!");

            if (logReader.HasErrors)
            {
                Console.WriteLine("There are errors... :(");
                foreach (var row in logReader.Errors)
                {
                    Console.WriteLine(row);
                }
            }
            else
            {
                Console.WriteLine($"Total games: {logReader.Games.Count}");
            }
        }
    }
}

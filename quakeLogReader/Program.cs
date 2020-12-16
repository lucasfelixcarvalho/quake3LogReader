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
            logReader.ReadLog(string.Empty);

            Console.WriteLine("Finished!");

            if (logReader.HasErrors)
            {
                Console.WriteLine("There are errors... :(");
                foreach (var row in logReader.Errors)
                {
                    Console.WriteLine(row);
                }
            }
        }
    }
}

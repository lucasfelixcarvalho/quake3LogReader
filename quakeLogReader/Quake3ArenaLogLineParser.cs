using System;
using System.Linq;
using quakeLogReader.Common;
using quakeLogReader.Dto;
using quakeLogReader.Handler;

namespace quakeLogReader
{
    public static class Quake3ArenaLogLineParser
    {        
        public static void ParseLine(string line, GameDto game)
        {
            ILogLineParser parser = Quake3ArenaLogLineParserFactory.GetParser(line);
            parser.Parse(line, game);
        }
    }
}

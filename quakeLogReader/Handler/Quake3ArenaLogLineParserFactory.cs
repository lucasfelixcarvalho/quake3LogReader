using quakeLogReader.Handler.Parsers;

namespace quakeLogReader.Handler
{
    public static class Quake3ArenaLogLineParserFactory
    {
        private static readonly string PlayerInformationIdentifier = "ClientUserinfoChanged:";
        private static readonly string KillIdentifier = "Kill:";
        private static readonly string PlayerDisconnectIdentifier = "ClientDisconnect:";

        public static ILogLineParser GetParser(string line)
        {
            ILogLineParser parser = null;
            if (line.Contains(PlayerInformationIdentifier))
            {
                parser = new PlayerInformationParser();
            }
            else if (line.Contains(KillIdentifier))
            {
                parser = new KillParser();
            }
            else if (line.Contains(PlayerDisconnectIdentifier))
            {
                parser = new PlayerDisconnectParser();
            }
            else
            {
                parser = new DefaultParser();
            }

            return parser;
        }
    }
}

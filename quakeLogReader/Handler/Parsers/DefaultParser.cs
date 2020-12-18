using quakeLogReader.Dto;

namespace quakeLogReader.Handler.Parsers
{
    public class DefaultParser : ILogLineParser
    {
        public void Parse(string line, GameDto game)
        {
            // do nothing
        }
    }
}

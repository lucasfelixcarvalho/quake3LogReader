using quakeLogReader.Dto;

namespace quakeLogReader.Handler
{
    public interface ILogLineParser
    {
        void Parse(string line, GameDto game);
    }
}

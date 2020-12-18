using System;
using quakeLogReader.Dto;

namespace quakeLogReader.Handler.Parsers
{
    public class PlayerDisconnectParser : ILogLineParser
    {
        private static readonly string PlayerDisconnectIdentifier = "ClientDisconnect:";

        public PlayerDisconnectParser()
        {
            // line example:
            // 13:27 ClientDisconnect: 4
        }
        
        public void Parse(string line, GameDto game)
        {
            int playerId = Convert.ToInt32(line.Split(PlayerDisconnectIdentifier)[1].Trim());
            game.Players.Remove(playerId);
            game.Score.Remove(playerId);
        }
    }
}

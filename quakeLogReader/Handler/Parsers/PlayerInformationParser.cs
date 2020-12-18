using System;
using quakeLogReader.Dto;

namespace quakeLogReader.Handler.Parsers
{
    public class PlayerInformationParser : ILogLineParser
    {
        private static readonly string PlayerInformationIdentifier = "ClientUserinfoChanged:";

        public PlayerInformationParser()
        {
            // line example:
            // 13:36 ClientUserinfoChanged: 2 n\Zeh\t\0\model\sarge/default\hmodel\sarge/default\g_redteam\\g_blueteam\\c1\1\c2\5\hc\100\w\0\l\0\tt\0\tl\0
        }

        public void Parse(string line, GameDto game)
        {
            string playerInformation = line.Split(PlayerInformationIdentifier)[1].Trim();                    
            int id = Convert.ToInt32(playerInformation.Split(@"n\")[0].Trim());
            string name = playerInformation.Split(@"n\")[1].Split(@"\t\")[0].Trim();
            
            if (!game.Players.TryAdd(id, name))
            {
                game.Players[id] = name; // player already exists, something changed (name could be one of them)
            }
        }
    }
}

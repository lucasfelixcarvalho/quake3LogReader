using System;
using System.Linq;
using quakeLogReader.Dto;

namespace quakeLogReader
{
    public static class Quake3ArenaLogLineParser
    {
        private static readonly int WorldKillerId = 1022;

        public static void ParseLine(string line, GameDto game)
        {
            if (line.Contains("ClientUserinfoChanged:"))
            {
                ParsePlayerInformations(line, game);
            }
            else if (line.Contains("Kill:"))
            {
                ParseKill(line, game);
            }
        }

        private static void ParsePlayerInformations(string line, GameDto game)
        {
            string playerInformation = line.Split("ClientUserinfoChanged:")[1].Trim();                    
            int id = Convert.ToInt32(playerInformation.Split(@"n\")[0].Trim());
            string name = playerInformation.Split(@"n\")[1].Split(@"\t\")[0].Trim();
            if (!game.Players.TryAdd(id, name))
            {
                game.Players[id] = name; // player already exists, something changed (name could be one of them)
            }
        }

        private static void ParseKill(string line, GameDto game)
        {
            game.TotalKills++;

            var killInformation = line.Split("Kill:");
            var playersAndWeapon = killInformation[1].Split(":")[0].Trim().Split(" ");

            int killerId = Convert.ToInt32(playersAndWeapon[0].Trim());
            int victim = Convert.ToInt32(playersAndWeapon[1].Trim());
            int weapon = Convert.ToInt32(playersAndWeapon[2].Trim());

            if (killerId == WorldKillerId) // when player is killed by world, it should lose one point
            {
                UpdatePlayerScore(game, victim, -1);
            }
            else
            {
                UpdatePlayerScore(game, killerId, 1);
            }
        }

        private static void UpdatePlayerScore(GameDto game, int playerId, int score)
        {
            if (game.Score.Any(x => x.Key == playerId))
                game.Score[playerId] += score;
            else
                game.Score.Add(playerId, score);
        }
    }
}

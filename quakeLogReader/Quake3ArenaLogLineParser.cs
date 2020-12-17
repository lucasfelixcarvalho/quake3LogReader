using System;
using System.Linq;
using quakeLogReader.Common;
using quakeLogReader.Dto;

namespace quakeLogReader
{
    public static class Quake3ArenaLogLineParser
    {
        private static readonly int WorldKillerId = 1022;
        private static readonly string PlayerInformationIdentifier = "ClientUserinfoChanged:";
        private static readonly string KillIdentifier = "Kill:";
        private static readonly string PlayerDisconnectIdentifier = "ClientDisconnect:";

        public static void ParseLine(string line, GameDto game)
        {
            if (line.Contains(PlayerInformationIdentifier))
            {
                ParsePlayerInformations(line, game);
            }
            else if (line.Contains(KillIdentifier))
            {
                ParseKill(line, game);
            }
            else if (line.Contains(PlayerDisconnectIdentifier))
            {
                ParsePlayerDisconnect(line, game);
            }
        }

        private static void ParsePlayerDisconnect(string line, GameDto game)
        {
            int playerId = Convert.ToInt32(line.Split(PlayerDisconnectIdentifier)[1].Trim());
            game.Players.Remove(playerId);
            game.Score.Remove(playerId);
        }

        private static void ParsePlayerInformations(string line, GameDto game)
        {
            string playerInformation = line.Split(PlayerInformationIdentifier)[1].Trim();                    
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

            var killInformation = line.Split(KillIdentifier);
            var playersAndWeapon = killInformation[1].Split(":")[0].Trim().Split(" ");

            int killerId = Convert.ToInt32(playersAndWeapon[0].Trim());
            int victimId = Convert.ToInt32(playersAndWeapon[1].Trim());
            int weaponId = Convert.ToInt32(playersAndWeapon[2].Trim());

            if (killerId == WorldKillerId) // when player is killed by world, it should lose one point
            {
                UpdatePlayerScore(game, victimId, -1);
            }
            else
            {
                UpdatePlayerScore(game, killerId, 1);
            }

            UpdateWeaponKillScore(game, weaponId);
        }

        private static void UpdatePlayerScore(GameDto game, int playerId, int score)
        {
            if (game.Score.Any(x => x.Key == playerId))
            {
                game.Score[playerId] += score;
            }
            else
            {
                game.Score.Add(playerId, score);
            }
        }

        private static void UpdateWeaponKillScore(GameDto game, int weaponId)
        {
            MeansOfDeath weapon = GetMeansOfDeathFromIntegerValue(weaponId);
            if (game.KillsByWeapon.Any(x => x.Key == weapon))
            {
                game.KillsByWeapon[weapon]++;
            }
            else
            {
                game.KillsByWeapon.Add(weapon, 1);
            }
        }

        private static MeansOfDeath GetMeansOfDeathFromIntegerValue(int weaponId)
        {
            if (Enum.IsDefined(typeof(MeansOfDeath), weaponId))
            {
                return (MeansOfDeath)weaponId;
            }
            else
            {
                return MeansOfDeath.MOD_UNKNOWN;
            }
        }
    }
}

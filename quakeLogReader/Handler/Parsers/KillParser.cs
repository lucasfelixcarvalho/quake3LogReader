using System;
using System.Linq;
using quakeLogReader.Common;
using quakeLogReader.Dto;

namespace quakeLogReader.Handler.Parsers
{
    public class KillParser : ILogLineParser
    {
        private static readonly int WorldKillerId = 1022;
        private static readonly string KillIdentifier = "Kill:";

        public KillParser()
        {
            // line example:
            // 14:15 Kill: 2 5 10: Zeh killed Assasinu Credi by MOD_RAILGUN
            // 14:02 Kill: 1022 5 22: <world> killed Assasinu Credi by MOD_TRIGGER_HURT
        }
        
        public void Parse(string line, GameDto game)
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

        private void UpdatePlayerScore(GameDto game, int playerId, int score)
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

        private void UpdateWeaponKillScore(GameDto game, int weaponId)
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

        private MeansOfDeath GetMeansOfDeathFromIntegerValue(int weaponId)
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

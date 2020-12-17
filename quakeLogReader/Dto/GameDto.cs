using System.Collections.Generic;
using quakeLogReader.Common;

namespace quakeLogReader.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        public int TotalKills { get; set; }
        public Dictionary<int, string> Players { get; private set; }
        public Dictionary<int, int> Score { get; private set; }
        public Dictionary<MeansOfDeath, int> KillsByWeapon { get; private set; }

        public GameDto(int id)
        {
            Id = id;
            Players = new Dictionary<int, string>();
            Score = new Dictionary<int, int>();
            KillsByWeapon = new Dictionary<MeansOfDeath, int>();
        }
    }
}

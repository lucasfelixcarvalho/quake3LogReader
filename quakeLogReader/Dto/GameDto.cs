using System.Collections.Generic;

namespace quakeLogReader.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        public int TotalKills { get; set; }
        public Dictionary<int, string> Players { get; private set; }
        public Dictionary<int, int> Kills { get; private set; }

        public GameDto(int id)
        {
            Id = id;
            Players = new Dictionary<int, string>();
            Kills = new Dictionary<int, int>();
        }
    }
}

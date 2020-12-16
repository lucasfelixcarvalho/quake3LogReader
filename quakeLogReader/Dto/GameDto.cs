using System.Collections.Generic;

namespace quakeLogReader.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        public int TotalKills { get; set; }
        public string[] Players { get; set; }
        public Dictionary<string, int> Kills { get; set; }

        public GameDto(int id)
        {
            Id = id;
        }
    }
}

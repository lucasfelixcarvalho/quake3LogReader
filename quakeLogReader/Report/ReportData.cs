using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using quakeLogReader.Dto;

namespace quakeLogReader.Report
{
    public class ReportData
    {
        public int Id { get; set; }
        public int TotalKills { get; set; }
        public string[] Players { get; private set; }
        public Dictionary<string, int> Kills { get; private set; }
        public Dictionary<string, int> KillsByMeans { get; private set; }

        public ReportData()
        {
            Players = new string[0];
            Kills = new Dictionary<string, int>();
            KillsByMeans = new Dictionary<string, int>();
        }

        public ReportData(GameDto game)
        {
            Id = game.Id;
            TotalKills = game.TotalKills;
            Players = game.Players.Values.ToArray();
            Kills = new Dictionary<string, int>();
            KillsByMeans = new Dictionary<string, int>();

            foreach (var row in game.Players)
            {
                string playerName = row.Value;
                int playerId = row.Key;
                int totalKills = game.Score.FirstOrDefault(x => x.Key == playerId).Value;

                Kills.Add(playerName, totalKills);
            }

            foreach (var row in game.KillsByWeapon)
            {
                string weaponName = row.Key.ToString();
                int totalKills = row.Value;

                KillsByMeans.Add(weaponName, totalKills);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, formatting: Newtonsoft.Json.Formatting.Indented);
        }
    }
}

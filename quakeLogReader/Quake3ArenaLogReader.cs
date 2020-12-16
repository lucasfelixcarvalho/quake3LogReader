using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using quakeLogReader.Dto;

namespace quakeLogReader
{
    public class Quake3ArenaLogReader
    {
        public List<string> Errors { get; private set; }
        public bool HasErrors => Errors.Any();
        public List<GameDto> Games { get; private set; }

        public Quake3ArenaLogReader()
        {
            Errors = new List<string>();
            Games = new List<GameDto>();
        }

        public void ReadLog(string fullPath)
        {
            try
            {
                Process(fullPath);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            Errors.Add($"Error: {ex.Message}. Stack: {ex.StackTrace}");
        }

        private void Process(string path)
        {
            if (IsInvalidToProcess(path))
                return;

            ProcessFile(path);
        }

        private void ProcessFile(string path)
        {
            StreamReader file = new StreamReader(path);
            string line;
            int gameCount = 0;
            GameDto game = new GameDto(gameCount);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("InitGame:"))
                {
                    Games.Add(game);
                    gameCount += 1;
                    game = new GameDto(gameCount);
                }
                if (line.Contains("ClientUserinfoChanged:"))
                {
                    string playerInformation = line.Split("ClientUserinfoChanged:")[1].Trim();                    
                    int id = Convert.ToInt32(playerInformation.Split(@"n\")[0].Trim());
                    string name = playerInformation.Split(@"n\")[1].Split(@"\t\")[0].Trim();
                    if (!game.Players.TryAdd(id, name)) // player already exists, change something, name could be one of them
                    {
                        game.Players[id] = name;
                    }
                }
                if (line.Contains("Kill:"))
                {
                    game.TotalKills++;
                    var killInformation = line.Split("Kill:");
                    string playersAndWeapon = killInformation[1].Split(":")[0].Trim();
                    int killerId = Convert.ToInt32(playersAndWeapon.Split(" ")[0].Trim());
                    int victim = Convert.ToInt32(playersAndWeapon.Split(" ")[1].Trim());
                    int weapon = Convert.ToInt32(playersAndWeapon.Split(" ")[2].Trim());

                    if (killerId == 1022) // when player is killed by world, it should lose one point
                    {
                        if (game.Kills.Any(x => x.Key == victim))
                            game.Kills[victim]--;
                        else
                            game.Kills.Add(victim, -1);
                    }
                    else
                    {
                        if (game.Kills.Any(x => x.Key == killerId))
                            game.Kills[killerId]++;
                        else
                            game.Kills.Add(killerId, 1);
                    }
                }
            }
            Games.RemoveAt(0);
            Games.Add(game);
        }

        private bool IsInvalidToProcess(string path)
        {
            if (string.IsNullOrEmpty(path))
                Errors.Add("Invalid path");

            if (!File.Exists(path))
                Errors.Add("File does not exist");

            return HasErrors;
        }
    }
}

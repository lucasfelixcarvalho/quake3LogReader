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
                if (line.Contains("Kill:"))
                {
                    game.TotalKills++;
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

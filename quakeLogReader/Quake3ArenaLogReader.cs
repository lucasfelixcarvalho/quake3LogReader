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
            {
                return;
            }

            ProcessFile(path);
            PostProcessing();
        }

        private void PostProcessing()
        {
            Games.RemoveAll(x => x == null);
        }

        private void ProcessFile(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                string line;
                int gameCount = 0;
                GameDto game = null;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("InitGame:")) // init game marks a new game, could not fit into the line parser...
                    {
                        Games.Add(game);
                        gameCount += 1;
                        game = new GameDto(gameCount);
                    }
                    else
                    {
                        Quake3ArenaLogLineParser.ParseLine(line, game);
                    }
                }
                
                Games.Add(game);
            }
        }

        private bool IsInvalidToProcess(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Errors.Add("Invalid path");
            }

            if (!File.Exists(path))
            {
                Errors.Add("File does not exist");
            }

            return HasErrors;
        }
    }
}

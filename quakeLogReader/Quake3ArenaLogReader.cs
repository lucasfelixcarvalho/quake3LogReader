using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace quakeLogReader
{
    public class Quake3ArenaLogReader
    {
        public List<string> Errors { get; private set; }
        public bool HasErrors => Errors.Any();

        public Quake3ArenaLogReader()
        {
            Errors = new List<string>();
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
            if (!IsValidToProcess(path))
                return;
        }

        private bool IsValidToProcess(string path)
        {
            if (string.IsNullOrEmpty(path))
                Errors.Add("Invalid path");

            if (!File.Exists(path))
                Errors.Add("File does not exist");

            return HasErrors;
        }
    }
}

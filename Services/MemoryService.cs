using System;
using System.Collections.Generic;
using System.Linq;

namespace POE_for_Prog_last_part.Services
{
    public class MemoryService
    {
        private readonly Dictionary<string, string> _memory = new Dictionary<string, string>();
        private readonly List<string> _activityLog = new List<string>();

        public void Remember(string key, string value)
        {
            _memory[key] = value;
            LogAction($"Remembered: {key}");
        }

        public string Recall(string key)
        {
            return _memory.TryGetValue(key, out var value) ? value : null;
        }

        public void LogAction(string action)
        {
            _activityLog.Add($"{DateTime.Now:HH:mm}: {action}");

            // Maintain log size (keep last 50 entries)
            if (_activityLog.Count > 50)
            {
                _activityLog.RemoveAt(0);
            }
        }

        public IEnumerable<string> GetRecentLogs()
        {
            // Fixed: Compatible with all .NET versions
            int startIndex = Math.Max(0, _activityLog.Count - 5);
            return _activityLog.Skip(startIndex).Take(5);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace POE_for_Prog_last_part.Services
{
    /// <summary>
    /// Provides memory storage and activity logging capabilities
    /// </summary>
    public class MemoryService
    {
        // Key-value store for persistent memory
        private readonly Dictionary<string, string> _memory = new Dictionary<string, string>();

        // Activity log with timestamped entries
        private readonly List<string> _activityLog = new List<string>();

        /// <summary>
        /// Stores a key-value pair in memory and logs the action
        /// </summary>
        /// <param name="key">Unique identifier for the information</param>
        /// <param name="value">Information to be stored</param>
        public void Remember(string key, string value)
        {
            _memory[key] = value;  // Add or update existing entry
            LogAction($"Remembered: {key}");  // Log the storage action
        }

        /// <summary>
        /// Retrieves a value from memory by key
        /// </summary>
        /// <param name="key">Identifier for the requested information</param>
        /// <returns>Stored value if found, otherwise null</returns>
        public string Recall(string key)
        {
            return _memory.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Adds an action to the activity log with timestamp
        /// </summary>
        /// <param name="action">Description of the performed action</param>
        public void LogAction(string action)
        {
            // Format: "HH:mm: Action description"
            _activityLog.Add($"{DateTime.Now:HH:mm}: {action}");

            // Maintain rolling log of last 50 entries
            if (_activityLog.Count > 50)
            {
                _activityLog.RemoveAt(0);  // Remove oldest entry
            }
        }

        /// <summary>
        /// Retrieves the 5 most recent log entries
        /// </summary>
        /// <returns>Recent log entries in chronological order</returns>
        public IEnumerable<string> GetRecentLogs()
        {
            // Calculate starting index for last 5 entries
            int startIndex = Math.Max(0, _activityLog.Count - 5);

            // Return last 5 entries (or fewer if log has <5 entries)
            return _activityLog.Skip(startIndex).Take(5);
        }
    }
}
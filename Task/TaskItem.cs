using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Task
{
    /// <summary>
    /// Represents a single task in a task management system
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Short title/name of the task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the task (optional)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the task has been completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Indicates whether a reminder is set for this task
        /// </summary>
        public bool HasReminder => ReminderDate != null;

        /// <summary>
        /// Date and time for task reminder (null if no reminder set)
        /// </summary>
        public DateTime? ReminderDate { get; set; }

        /// <summary>
        /// Initializes a new task item
        /// </summary>
        /// <param name="id">Unique task identifier</param>
        /// <param name="title">Short name for the task</param>
        public TaskItem(int id, string title)
        {
            Id = id;
            Title = title;
            IsCompleted = false;  // New tasks are incomplete by default
        }

        /// <summary>
        /// Returns formatted string representation of the task
        /// </summary>
        /// <returns>String showing ID, title, and completion status</returns>
        public override string ToString()
        {
            // Format: "1. Task Title ✓" (with checkmark for completed tasks)
            return $"{Id}. {Title} {(IsCompleted ? "✓" : "")}";
        }
    }
}
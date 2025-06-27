using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Task
{
    /// <summary>
    /// Manages a collection of tasks with CRUD operations and additional features
    /// </summary>
    public class TaskManager
    {
        // Stores all tasks in memory
        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        // Next available task ID (auto-incremented)
        private int _nextId = 1;

        // Maximum allowed tasks (prevent unlimited growth)
        private const int MaxTasks = 10;

        /// <summary>
        /// Adds a new task to the manager
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <returns>Created task or null if limit reached</returns>
        public TaskItem AddTask(string title)
        {
            // Enforce maximum task limit
            if (_tasks.Count >= MaxTasks) return null;

            // Create task with auto-increment ID
            var task = new TaskItem(_nextId++, title);
            _tasks.Add(task);
            return task;
        }

        /// <summary>
        /// Marks a task as completed
        /// </summary>
        /// <param name="taskId">ID of task to complete</param>
        /// <returns>True if task was found and marked complete</returns>
        public bool CompleteTask(int taskId)
        {
            var task = GetTask(taskId);

            // Fail if task doesn't exist or already completed
            if (task == null || task.IsCompleted) return false;

            task.IsCompleted = true;
            return true;
        }

        /// <summary>
        /// Removes a task from the manager
        /// </summary>
        /// <param name="taskId">ID of task to remove</param>
        /// <returns>True if task was found and removed</returns>
        public bool RemoveTask(int taskId)
        {
            var task = GetTask(taskId);
            return task != null && _tasks.Remove(task);
        }

        /// <summary>
        /// Clears all completed tasks from the manager
        /// </summary>
        public void ClearCompletedTasks()
        {
            // Remove all tasks where IsCompleted is true
            _tasks.RemoveAll(t => t.IsCompleted);
        }

        /// <summary>
        /// Retrieves a single task by ID
        /// </summary>
        /// <param name="taskId">ID of task to find</param>
        /// <returns>TaskItem or null if not found</returns>
        public TaskItem GetTask(int taskId)
        {
            // Find first task matching ID (case-sensitive)
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        /// <summary>
        /// Retrieves all tasks with optional completion filter
        /// </summary>
        /// <param name="includeCompleted">Whether to include completed tasks</param>
        /// <returns>Filtered list of tasks</returns>
        public IEnumerable<TaskItem> GetTasks(bool includeCompleted)
        {
            return includeCompleted
                ? _tasks  // All tasks
                : _tasks.Where(t => !t.IsCompleted);  // Only active tasks
        }

        /// <summary>
        /// Updates description for a specific task
        /// </summary>
        /// <param name="taskId">ID of task to update</param>
        /// <param name="description">New description text</param>
        public void SetDescription(int taskId, string description)
        {
            var task = GetTask(taskId);
            if (task != null) task.Description = description;
        }

        /// <summary>
        /// Sets or updates a reminder for a task
        /// </summary>
        /// <param name="taskId">ID of task to update</param>
        /// <param name="reminderDate">DateTime for reminder (pass null to clear)</param>
        public void SetReminder(int taskId, DateTime reminderDate)
        {
            var task = GetTask(taskId);
            if (task != null) task.ReminderDate = reminderDate;
        }
    }
}
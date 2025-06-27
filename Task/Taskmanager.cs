using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Task
{
    public class TaskManager
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();
        private int _nextId = 1;
        private const int MaxTasks = 10;

        public TaskItem AddTask(string title)
        {
            if (_tasks.Count >= MaxTasks) return null;

            var task = new TaskItem(_nextId++, title);
            _tasks.Add(task);
            return task;
        }

        public bool CompleteTask(int taskId)
        {
            var task = GetTask(taskId);
            if (task == null || task.IsCompleted) return false;

            task.IsCompleted = true;
            return true;
        }

        public bool RemoveTask(int taskId)
        {
            var task = GetTask(taskId);
            return task != null && _tasks.Remove(task);
        }

        public void ClearCompletedTasks()
        {
            _tasks.RemoveAll(t => t.IsCompleted);
        }

        public TaskItem GetTask(int taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public IEnumerable<TaskItem> GetTasks(bool includeCompleted)
        {
            return includeCompleted
                ? _tasks
                : _tasks.Where(t => !t.IsCompleted);
        }

        public void SetDescription(int taskId, string description)
        {
            var task = GetTask(taskId);
            if (task != null) task.Description = description;
        }

        public void SetReminder(int taskId, DateTime reminderDate)
        {
            var task = GetTask(taskId);
            if (task != null) task.ReminderDate = reminderDate;
        }
    }
}
  
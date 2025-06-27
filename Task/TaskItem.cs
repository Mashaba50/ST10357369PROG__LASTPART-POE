using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Task
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasReminder => ReminderDate != null;
        public DateTime? ReminderDate { get; set; }

        public TaskItem(int id, string title)
        {
            Id = id;
            Title = title;
            IsCompleted = false;
        }

        public override string ToString()
        {
            return $"{Id}. {Title} {(IsCompleted ? "✓" : "")}";
        }
    }
}
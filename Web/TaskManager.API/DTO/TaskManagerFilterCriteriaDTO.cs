using System;

namespace TaskManager.API.DTO
{
    public class TaskManagerFilterCriteriaDTO
    {
        public int? ParentTaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PriorityFrom { get; set; }
        public int? PriorityTo { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int? ParentTaskId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskName is required")]
        public string TaskName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Priority is required")]
        [Range(minimum: 0, maximum: 30, ErrorMessage = "Priority value is out of range, must be between 0 and 30")]
        public int Priority { get; set; }

        public bool? IsTaskComplete { get; set; }

        public Task ParentTask { get; set; }
    }
}